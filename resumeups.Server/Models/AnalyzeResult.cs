namespace resumeups.Server.Models
{
    public class AnalyzeResult
    {
        public MatchingResult Matching { get; set; } = new();
        public FeedbackResult Feedback { get; set; } = new();
    }
}
