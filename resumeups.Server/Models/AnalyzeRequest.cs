namespace resumeups.Server.Models
{
    public class AnalyzeRequest
    {
        public string Resume { get; set; }
        public string JobDescription { get; set; }
        public string Language { get; set; } = "auto";
    }
}
