using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Sender;
using Takenet.MessagingHub.Client.Listener;
using System.Diagnostics;
using System;

namespace FlowTest.Bot.NetFramework
{
	public class Startup : IStartable
	{
		private readonly IMessagingHubSender _sender;
		private readonly BotSettings _settings;

		public Startup(IMessagingHubSender sender, BotSettings settings)
		{
			_sender = sender;
			_settings = settings;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			return Task.CompletedTask;
		}
	}
}
