using System;
using System.Collections.Generic;
using System.Text;
using FlowTestClient.Models;

namespace FlowTestClient
{
    public class MessageProvider : IMessageProvider
    {
        public TestMessage BuildMessage(string input)
        {
            TestMessage message = null;

            input = input.Replace("\\n", Environment.NewLine).Trim();
            var direction = input[0];
            if (direction == '>') // To bot (request)
            {
                message = new ToBotMessage()
                {
                    RawTextContent = input.Substring(1).Trim()
                };
            }
            else if (direction == '<') // From bot (response)
            {
                message = new FromBotMessage()
                {
                    RawTextContent = input.Substring(1).Trim()
                };
            }
            else
            {
                message = new UnexpectedTestMessage();
            }
            return message;
        }
    }
}
