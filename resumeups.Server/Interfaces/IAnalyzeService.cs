using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IAnalyzeService
    {
        Task<AnalyzeResult> Analyze(AnalyzeRequest request);
    }
}
