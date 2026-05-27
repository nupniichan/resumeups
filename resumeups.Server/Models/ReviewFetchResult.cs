using System.Collections.Generic;

namespace resumeups.Server.Models
{
    public sealed class ReviewFetchResult
    {
        public ReviewStats PartialStats { get; set; } = new();
        public List<string> RawReviews { get; set; } = new();
    }
}
