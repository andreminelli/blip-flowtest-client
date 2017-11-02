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

            var docs = _settings.TextDocs;

            var textDoc = docs.FirstOrDefault(d => d.Command.Equals(text, StringComparison.OrdinalIgnoreCase));

            if (textDoc == default(TestDoc))
            {
                await _sender.SendMessageAsync("none", message.From, cancellationToken);
                return;
            }

            var taskList = new List<Task>();
            textDoc.PlainDocument
                .Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries)
                .ForEach(e =>
                {
                    taskList.Add(_sender.SendMessageAsync(e, message.From, cancellationToken));
                });

            foreach (var task in taskList)
            {
                await task;
            }
        }
    }
}
