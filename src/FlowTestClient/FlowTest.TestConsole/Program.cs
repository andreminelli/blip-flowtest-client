using FlowTest.Bot.NetFramework;
using FlowTest.Channel.BlipTestHost;
using FlowTest.Client;
using FlowTest.Contracts;
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
            var host = new TestHost(typeof(Startup).Assembly, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
            var eventTrackExtension = Substitute.For<IEventTrackExtension>();
            //var container = host.AddRegistrationAndStartAsync(eventTrackExtension).GetAwaiter().GetResult();

            host.StartAsync().GetAwaiter().GetResult();


            ITestChannel channel = new BlipTestHostChannel(host, new Contracts.Models.TestCaseSettings
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
