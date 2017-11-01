using FlowTest.Contracts.Models;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowTest.Contracts
{
    public interface ITestChannel
    {

        //Test to bot message
        Task SendTextAsync(string text, CancellationToken cancellationToken);

        //Test from bot message
        Task<Document> ReceiveAsync(CancellationToken cancellationToken);

    }
}
