using FlowTest.Bot.NetFramework;
using FlowTest.Channel.BlipTestHost;
using FlowTest.Client;
using FlowTest.Contracts;
using FlowTest.Contracts.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client.Testing;
using Takenet.MessagingHub.Client.Extensions.EventTracker;

namespace FlowTest.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var blipTestHostChannelSettings = new BlipTestHostChannelSettings
            {
                AssemblyReference = typeof(Startup).Assembly,
                MessageTimeout = TimeSpan.FromMinutes(2),
                NotificationTimeout = TimeSpan.FromMinutes(2)
            };

            ITestChannel channel = new BlipTestHostChannel(blipTestHostChannelSettings, new TestCaseSettings
            {
                User = "random",
                Channel = "Messenger"
            });

            FlowTestClient client = new FlowTestClient(channel);

            using (var cts = new CancellationTokenSource())
            {
                var response = client.RunAsync(args[0], cts.Token).Result;
                Console.WriteLine($"Result: {response.IsSuccessfully}");
            }

            Console.ReadKey();

        }
    }
}
