using FlowTest.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lime.Protocol;
using System.Threading;
using Take.Blip.Client.Testing;
using FlowTest.Contracts.Models;
using Lime.Messaging.Contents;

namespace FlowTest.Channel.BlipTestHost
{
    public class BlipTestHostChannel : ITestChannel
    {
        private readonly TestHost _testHost;
        private readonly TestCaseSettings _settings;

        public BlipTestHostChannel(
            TestHost testHost,
            TestCaseSettings settings
            )
        {
            _testHost = testHost;
            _settings = settings;
        }

        public async Task<Document> ReceiveAsync(CancellationToken cancellationToken)
        {
            var message = await _testHost.GetResponseIgnoreFireAndForgetAsync(false);
            return message.Content;
        }

        public async Task SendTextAsync(string text, CancellationToken cancellationToken)
        {
            var message = new Message(EnvelopeId.NewId())
            {
                Content = PlainText.Parse(text),
                From = GetUserNode(),
            };
            await _testHost.DeliverIncomingMessageAsync(message);
        }

        private Node GetUserNode()
        {
            if(_settings.Channel == "Messenger")
            {
                if(_settings.User == "random")
                {
                    return Dummy.MessengerUser();
                }
                return Dummy.MessengerUser(_settings.User);
            }

            if (_settings.Channel == "BlipChat")
            {
                if (_settings.User == "random")
                {
                    return Dummy.BlipSdkUser();
                }
                return Dummy.BlipSdkUser(_settings.User);
            }

            return Node.Parse($"{_settings.User}@{_settings.Channel}");
        }
    }
}
