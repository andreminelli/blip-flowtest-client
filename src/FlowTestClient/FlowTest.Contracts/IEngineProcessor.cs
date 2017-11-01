using System.Threading;
using System.Threading.Tasks;
using FlowTest.Contracts.Models;

namespace FlowTest.Contracts
{
    public interface IEngineProcessor
    {
        Task<FlowTestResult> ExecuteTestCaseAsync(FlowTestCase testCase, CancellationToken cancellationToken);
    }
}