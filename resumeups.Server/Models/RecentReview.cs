namespace resumeups.Server.Models
{
    public sealed class RecentReview
    {
        public string Title { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public double? Rating { get; set; }
        public string Pros { get; set; } = string.Empty;
        public string Cons { get; set; } = string.Empty;
    }
}
