namespace resumeups.Server.Models
{
    public class FeedbackResult
    {
        public int FeedbackScore { get; set; }
        public string Summary { get; set; }
        public List<string> Issues { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();
    }
}
