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

        private Document GetDocument(string raw, string type)
        {
            if ("text" == type)
            {
                return PlainText.Parse(raw);
            }

            if ("menu" == type || "quickreply" == type)
            {
                var parts = raw.Split('|');
                var select = new Select
                {
                    Text = parts[0],
                    Options = parts
                                .Where(p => p != parts[0])
                                .Select(o => new SelectOption { Text = o, Value = PlainText.Parse(o) })
                                .ToArray(),
                    Scope = ("quickreply" == type) ? SelectScope.Immediate : SelectScope.Transient
                };
                return select;
            }

            if ("carousel_img" == type)
            {
                var docColl = new DocumentCollection
                {
                    ItemType = DocumentSelect.MediaType
                };

                var items = new List<DocumentSelect>();
                var cards = raw.Split('%');

                foreach (var item in cards)
                {
                    var parts = item.Split('|');
                    var header = new List<string>();
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        header.Add(parts[i]);
                    }
                    var btns = parts[parts.Length - 1].Split('&');

                    var docSel = new DocumentSelect
                    {
                        Header = new DocumentContainer
                        {
                            Value = new MediaLink
                            {
                                Title = header[1],
                                Text = header[2],
                                Uri = new Uri("http://site.com/" + header[0] + ".jpg"),
                                Type = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg)
                            }
                        },
                        Options = btns.Select(b =>
                            new DocumentSelectOption
                            {
                                Label = new DocumentContainer
                                {
                                    Value = new PlainText { Text = b }
                                }
                            }
                        ).ToArray()
                    };
                    items.Add(docSel);
                }

                docColl.Items = items.ToArray();
                docColl.Total = docColl.Items.Length;

                return docColl;
            }

            if ("carousel_txt" == type)
            {
                var docColl = new DocumentCollection
                {
                    ItemType = DocumentSelect.MediaType,
                };

                var items = new List<DocumentSelect>();
                var cards = raw.Split('%');

                foreach (var item in cards)
                {
                    var parts = item.Split('|');
                    var header = new List<string>();
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        header.Add(parts[i]);
                    }
                    var btns = parts[parts.Length - 1].Split('&');

                    var docSel = new DocumentSelect
                    {
                        Header = new DocumentContainer
                        {
                            Value = new PlainText
                            {
                                Text = header[0],
                            }
                        },
                        Options = btns.Select(b =>
                            new DocumentSelectOption
                            {
                                Label = new DocumentContainer
                                {
                                    Value = new PlainText { Text = b }
                                }
                            }
                        ).ToArray()
                    };
                    items.Add(docSel);
                }

                docColl.Items = items.ToArray();
                docColl.Total = docColl.Items.Length;

                return docColl;
            }

            return PlainText.Parse(raw);
        }
    }
}
