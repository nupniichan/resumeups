using System.Collections.Generic;

namespace resumeups.Server.Models
{
    public sealed class Note8CandidateData
    {
        public bool HasResults { get; set; }
        public string CandidatesJson { get; set; } = "[]";
        public Dictionary<string, Note8CompanyMeta> MetaBySlug { get; set; } = new();
    }
}
