using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlowTestClient.Models;
using System.IO;

namespace FlowTestClient
{
    public class ScriptReader : IScriptReader
    {
        public async Task<FlowTest> ExtractFlowTestAsync(string fullFileName, CancellationToken cancellationToken)
        {
            var flowTest = new FlowTest();
            var lines = new Queue<string>();

            using (var fileStream = File.OpenRead(fullFileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
                {
                    lines.Enqueue(line.Replace("\\n", "\n"));
                }

            }

            var user = lines.Dequeue();
            user = user.Replace("User:", string.Empty).Trim();
            flowTest.User = user;

            var channel = lines.Dequeue();
            channel = channel.Replace("Channel:", string.Empty).Trim();
            flowTest.Channel = channel;

            var identifier = lines.Dequeue();
            identifier = identifier.Replace("Identifier:", string.Empty).Trim();
            flowTest.Identifier = identifier;

            var accessKey = lines.Dequeue();
            accessKey = accessKey.Replace("AccessKey:", string.Empty).Trim();
            flowTest.AccessKey = accessKey;
            
            while(lines.Count > 0)
            {
                var item = lines.Dequeue();
                item = item.TrimStart();

                var direction = item[0];

                TestMessage message = null;

                if(direction == '>') // To bot (request)
                {
                    message = new ToBotMessage()
                    {
                        RawTextContent = item.Substring(1).Trim()
                    };
                }
                else if(direction == '<') // From bot (response)
                {
                    message = new FromBotMessage()
                    {
                        RawTextContent = item.Substring(1).Trim()
                    };
                }
                else
                {
                    message = new UnexpectedTestMessage();
                }

                flowTest.AddTestMessage(message);

            }

            return flowTest;
        }
    }
}
