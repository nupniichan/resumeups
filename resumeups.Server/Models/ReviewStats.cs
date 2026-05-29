using System.Collections.Generic;

namespace resumeups.Server.Models
{
    public sealed class ReviewStats
    {
        public bool Found { get; set; }
        public double? Rating { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string ReviewsUrl { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();

        // Indeed and Glassdoor is kinda different
        public string ReviewsCount { get; set; } = string.Empty;
        public double? WorkLifeBalance { get; set; }
        public double? PayAndBenefits { get; set; }
        public double? JobSecurityAndAdvancement { get; set; }
        public double? Management { get; set; }
        public double? Culture { get; set; }
        public double? DiversityAndInclusion { get; set; }
        public List<DemographicRating> DemographicRatings { get; set; } = new();
        public List<string> ReviewTitles { get; set; } = new();
    }
}
