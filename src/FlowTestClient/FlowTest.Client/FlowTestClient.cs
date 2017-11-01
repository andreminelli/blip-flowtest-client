using FlowTest.Contracts;
using FlowTest.Contracts.Models;
using FlowTest.Engine;
using FlowTest.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowTest.Client
{
    public class FlowTestClient
    {
        private readonly IScriptReader _scriptReader;
        private readonly ITestChannel _testChannel;
        private readonly IEngineProcessor _engineProcessor;

        public FlowTestClient(
            ITestChannel testChannel
            )
        {
            _scriptReader = new ScriptReader(new MessageProvider());
            _testChannel = testChannel;
            _engineProcessor = new EngineProcessor(testChannel);
        }

        public async Task<FlowTestResult> RunAsync(string filename, CancellationToken cancellationToken)
        {
            var flowtest = await _scriptReader.ExtractFlowTestAsync(filename, cancellationToken);
            return await _engineProcessor.ExecuteTestCaseAsync(flowtest, cancellationToken);
        }

    }
}
