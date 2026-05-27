namespace resumeups.Server.Models
{
    public sealed class ReviewCongTyCandidateData
    {
        public bool HasResults { get; set; }
        public string CandidatesJson { get; set; } = "[]";
    }
}
