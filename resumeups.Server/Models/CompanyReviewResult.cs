namespace resumeups.Server.Models
{
    public sealed class CompanyReviewResult
    {
        public string CompanyName { get; set; } = string.Empty;
        public ReviewStats Note8 { get; set; } = new();
        public ReviewStats ReviewCongTy { get; set; } = new();
        public ReviewStats Indeed { get; set; } = new();
    }
}
