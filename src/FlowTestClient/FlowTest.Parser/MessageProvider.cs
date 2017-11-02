using FlowTest.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FlowTest.Parser
{
    public class MessageProvider : IMessageProvider
    {
        public static RegexOptions DefaultRegexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled;

        private readonly Regex _menuRegex;
        private readonly Regex _quickReplyRegex;
        private readonly Regex _simpleOptionsRegex;
        private readonly Regex _carouselRegex;

        public MessageProvider()
        {
            _menuRegex = new Regex(@"\[((MN)|(MENU))\]", DefaultRegexOptions);
            _quickReplyRegex = new Regex(@"\[((QR)|(QUICKREPLY))\]", DefaultRegexOptions);
            _simpleOptionsRegex = new Regex(@"<.*>", DefaultRegexOptions);
            _carouselRegex = new Regex(@"\[((CS)|(CAROUSEL))\]", DefaultRegexOptions);
        }

        public TestMessage BuildMessage(string input)
        {
            TestMessage message = null;

            input = input.Replace("##", Environment.NewLine).Trim();
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
                message = GetMessageFromContent(input.Substring(1).Trim());
            }
            else
            {
                message = new UnexpectedTestMessage();
            }
            return message;
        }

        private FromBotMessage GetMessageFromContent(string content)
        {
            var fromBotMessage = new FromBotMessage
            {
                RawTextContent = content,
                ContentType = "text"
            };

            if (string.IsNullOrWhiteSpace(content))
            {
                fromBotMessage.ContentType = "any";
            }
            else
            if (_menuRegex.IsMatch(content))
            {
                fromBotMessage.ContentType = "menu";
                content = _menuRegex.Replace(content, string.Empty).Trim();
                if (_simpleOptionsRegex.IsMatch(content))
                {
                    var optsRaw = _simpleOptionsRegex.Match(content).Value.Trim();
                    var opts = optsRaw.Substring(1, optsRaw.Length - 2).Split('#');
                    fromBotMessage.Options = opts.ToList();
                    content = _simpleOptionsRegex.Replace(content, string.Empty).Trim();
                }
            }
            else
            if (_quickReplyRegex.IsMatch(content))
            {
                fromBotMessage.ContentType = "quickreply";
                content = _quickReplyRegex.Replace(content, string.Empty).Trim();
                if (_simpleOptionsRegex.IsMatch(content))
                {
                    var optsRaw = _simpleOptionsRegex.Match(content).Value.Trim();
                    var opts = optsRaw.Substring(1, optsRaw.Length - 2).Split('#');
                    fromBotMessage.Options = opts.ToList();
                    content = _simpleOptionsRegex.Replace(content, string.Empty).Trim();
                }
            }
            else
            if (_carouselRegex.IsMatch(content))
            {
                fromBotMessage.ContentType = "carousel";
                content = _carouselRegex.Replace(content, string.Empty).Trim();
            }

            fromBotMessage.Text = content;
            return fromBotMessage;
        }
    }
}
