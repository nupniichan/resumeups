using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Services
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly IMatchingService _matchingService;
        private readonly IFeedbackService _feedbackService;
        public AnalyzeService(IMatchingService matchingService, IFeedbackService feedbackService)
        {
            _matchingService = matchingService;
            _feedbackService = feedbackService;
        }

        public async Task<AnalyzeResult> Analyze(AnalyzeRequest request)
        {
            var matching = await _matchingService.AnalyzeMatching(request.Resume, request.JobDescription);
            var feedback = await _feedbackService.AnalyzeFeedback(request.Resume, request.JobDescription, matching, request.Language);

            return new AnalyzeResult
            {
                Matching = matching,
                Feedback = feedback
            };
        }
    }
}
