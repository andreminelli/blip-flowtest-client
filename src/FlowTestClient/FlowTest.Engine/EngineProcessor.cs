using FlowTest.Contracts;
using FlowTest.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Lime.Messaging.Contents;
using System.Text.RegularExpressions;

namespace FlowTest.Engine
{
    public class EngineProcessor : IEngineProcessor
    {
        private readonly ITestChannel _channel;

        public EngineProcessor(
            ITestChannel channel
            )
        {
            _channel = channel;
        }

        public async Task<FlowTestResult> ExecuteTestCaseAsync(FlowTestCase testCase, CancellationToken cancellationToken)
        {
            var result = new FlowTestResult
            {
                IsSuccessfully = true
            };
            
            var messages = testCase.TestMessagesQueue;
            
            while(messages.Any())
            {
                var message = messages.Dequeue();

                if(message is ToBotMessage)
                {
                    await _channel.SendTextAsync(message.RawTextContent, cancellationToken);
                    continue;
                }

                if(message is FromBotMessage)
                {
                    var document = await _channel.ReceiveAsync(cancellationToken);
                    var localTestResult = CheckDocument(document, message as FromBotMessage);
                    if (!localTestResult.Works)
                        result.IsSuccessfully = false;
                    continue;
                }

                //Ignore the rest
                
            }

            return result;
        }

        private TestResult CheckDocument(Document document, FromBotMessage fromBotMessage)
        {
            var testResult = new TestResult();

            var text = document.ToString();

            var regex = new Regex(fromBotMessage.RawTextContent);

            var match = regex.Match(text);

            testResult.Works = match.Success;

            return testResult;
        }
    }
}
