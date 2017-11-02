using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Lime.Messaging.Contents;

namespace FlowTest.Bot.NetFramework
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private readonly BotSettings _settings;

        public PlainTextMessageReceiver(
            IMessagingHubSender sender,
            BotSettings settings
            )
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            var text = message.Content.ToString();
            var response = string.Empty;

            var docs = _settings.TestDoc;

            var testDoc = docs.FirstOrDefault(d => d.Command.Equals(text, StringComparison.OrdinalIgnoreCase));

            if (testDoc == default(TestDoc))
            {
                await _sender.SendMessageAsync("none", message.From, cancellationToken);
                return;
            }

            var taskList = new List<Task>();
            testDoc.RawDocument
                .Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                .ForEach(e =>
                {
                    taskList.Add(_sender.SendMessageAsync(GetDocument(e, testDoc.Type), message.From, cancellationToken));
                });

            foreach (var task in taskList)
            {
                await task;
            }
        }

        private Document GetDocument(string e, string type)
        {
            if("text" == type)
            {
                return PlainText.Parse(e);
            }

            if ("menu" == type || "quickreply" == type)
            {
                var parts = e.Split('|');
                var select = new Select
                {
                    Text = parts[0],
                    Options = parts
                                .Where(p => p != parts[0])
                                .Select(o => new SelectOption { Text = o, Value = PlainText.Parse(o)})
                                .ToArray(),
                    Scope = ("quickreply" == type) ? SelectScope.Immediate : SelectScope.Transient
                };
                return select;
            }

            return PlainText.Parse(e);
        }
    }
}
