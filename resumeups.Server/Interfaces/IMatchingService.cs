using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IMatchingService
    {
        Task<MatchingResult> AnalyzeMatching(string resume, string jd);
    }
}
