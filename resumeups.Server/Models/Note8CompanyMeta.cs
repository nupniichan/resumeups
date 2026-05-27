namespace resumeups.Server.Models
{
    public sealed class Note8CompanyMeta
    {
        public int Id { get; set; }
        public double? AverageRating { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }
}
