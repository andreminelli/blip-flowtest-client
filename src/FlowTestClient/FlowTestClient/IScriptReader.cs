using FlowTestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowTestClient
{
    public interface IScriptReader
    {
        // Ler o arquivo e retornar o caso de teste
        Task<FlowTest> ExtractFlowTestAsync(string fullFileName, CancellationToken cancellationToken = default(CancellationToken));
    }
}
