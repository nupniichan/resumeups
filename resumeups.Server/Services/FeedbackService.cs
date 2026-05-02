using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Services
{
    public class FeedbackService : IFeedbackService
    {
        public async Task<FeedbackResult> AnalyzeFeedback(string resume, string jobDescription, MatchingResult matching)
        {
            throw new NotImplementedException();
        }
    }
}
