
using FlowTest.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowTest.Parser
{
    public interface IScriptReader
    {
        // Ler o arquivo e retornar o caso de teste
        Task<FlowTestCase> ExtractFlowTestAsync(string fullFileName, CancellationToken cancellationToken);
    }
}
