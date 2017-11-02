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

            while (messages.Any())
            {
                var message = messages.Dequeue();

                if (message is ToBotMessage)
                {
                    await _channel.SendTextAsync(message.RawTextContent, cancellationToken);
                    continue;
                }

                if (message is FromBotMessage)
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

        private Regex GetRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }


        private TestResult CheckDocument(Document document, FromBotMessage fromBotMessage)
        {
            if (fromBotMessage.ContentType == "any") return new TestResult { Works = true };

            if (document is PlainDocument)
            {
                return CheckTextDocument(document.ToString(), fromBotMessage);
            }

            if (document is PlainText)
            {
                return CheckTextDocument((document as PlainText).Text, fromBotMessage);
            }

            if (document is Select)
            {
                return CheckMenuDocument((document as Select), fromBotMessage);
            }

            if (document is DocumentCollection)
            {
                return CheckCarouselDocument((document as DocumentCollection), fromBotMessage);
            }

            return null;
        }

        private TestResult CheckTextDocument(string textDocument, FromBotMessage fromBotMessage)
        {
            var testResult = new TestResult();
            var regex = GetRegex(fromBotMessage.RawTextContent);
            testResult.Works = regex.IsMatch(textDocument);

            return testResult;
        }

        private TestResult CheckMenuDocument(Select select, FromBotMessage fromBotMessage)
        {
            var textRegex = GetRegex(fromBotMessage.Text);
            var textOk = textRegex.IsMatch(select.Text);

            var optsOk = true;

            if (fromBotMessage.OptionsNumber > 0)
            {
                if (fromBotMessage.OptionsNumber != select.Options.Length)
                {
                    optsOk = false;
                }
                else
                {
                    for (int i = 0; i < select.Options.Length; i++)
                    {
                        var optRegex = GetRegex(fromBotMessage.Options[i]);
                        if (!optRegex.IsMatch(select.Options[i].Text))
                        {
                            optsOk = false;
                        }
                    }
                }

            }

            var typeOk = select.Scope == SelectScope.Immediate ? (fromBotMessage.ContentType == "quickreply") : (fromBotMessage.ContentType == "menu");

            return new TestResult { Works = textOk && optsOk && typeOk };

        }

        private TestResult CheckCarouselDocument(DocumentCollection documentCollection, FromBotMessage fromBotMessage)
        {
            return new TestResult { Works = fromBotMessage.ContentType == "carousel" };
        }
    }
}
