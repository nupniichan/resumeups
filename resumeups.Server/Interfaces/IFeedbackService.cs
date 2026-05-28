using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResult> AnalyzeFeedback(string resume, string jobDescription, MatchingResult matching, string language = "auto");
    }
}
