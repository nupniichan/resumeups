namespace resumeups.Server.Models
{
    public class MatchingResult
    {
        public int MatchScore { get; set; }
        public List<string> KeywordsMatching { get; set; } = new();
        public List<string> KeywordsMissing { get; set; } = new();
    }
}
