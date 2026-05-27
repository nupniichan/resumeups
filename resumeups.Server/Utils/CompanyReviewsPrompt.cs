namespace resumeups.Server.Utils
{
    public static class CompanyReviewsPrompt
    {
        public const string MultiSiteMatchPrompt = @"Return ONLY a valid minified JSON object. No markdown, no extra text, no explanation.

        You are an AI matching assistant. Given the user's search query and candidate lists from TWO different job review sites, identify the best match on each site independently.

        ### RULES:
        1. Compare the searched name against each site's candidates (text, slug, website).
        2. Matches can be English vs Vietnamese name, or abbreviation (e.g. 'EY' vs 'Ernst & Young', 'Fsoft' vs 'FPT Software').
        3. If a high-confidence match is found for a site, return its slug.
        4. If no candidate represents the searched company on a site, return ""NOT_FOUND"" for that site.
        5. Evaluate each site's candidates independently — a match on one site does not imply a match on the other.

        ### OUTPUT FORMAT:
        {
          ""note8Slug"": ""best-matching-slug-or-NOT_FOUND"",
          ""reviewCongTySlug"": ""best-matching-slug-or-NOT_FOUND""
        }

        ### DATA:
        [SEARCH_QUERY]: {SEARCH_QUERY}
        [NOTE8_CANDIDATES]: {NOTE8_CANDIDATES}
        [REVIEWCONGTY_CANDIDATES]: {REVIEWCONGTY_CANDIDATES}";

        public const string MultiSiteSummarizationPrompt = @"Return ONLY valid JSON. Do not wrap in ```json codeblocks. No markdown, no extra text, no explanation.

        You are a senior HR consultant analyzing employee reviews from TWO different job review platforms. Synthesize the reviews from each platform separately into professional summaries.

        ### RULES:
        1. Write everything in natural, professional Vietnamese (addressing the candidate as 'bạn').
        2. Keep each summary concise (2-3 sentences).
        3. Extract up to 4-5 key Pros (ưu điểm) and Cons (nhược điểm) per platform based on common themes.
        4. Provide 3-4 actionable Recommendations/Advice (lời khuyên) per platform for job seekers.
        5. DO NOT invent ratings, numbers, or details not in the raw review text.
        6. If reviews for a platform are marked ""NO_REVIEWS"", return these defaults for that platform:
           - summary: ""Chưa có đánh giá cụ thể nào được ghi nhận cho công ty này.""
           - pros/cons/recommendations: [""Chưa có ghi nhận""]

        ### OUTPUT FORMAT:
        {
          ""note8"": {
            ""summary"": ""..."",
            ""pros"": [""ưu điểm 1"", ""ưu điểm 2""],
            ""cons"": [""nhược điểm 1"", ""nhược điểm 2""],
            ""recommendations"": [""lời khuyên 1"", ""lời khuyên 2""]
          },
          ""reviewCongTy"": {
            ""summary"": ""..."",
            ""pros"": [""ưu điểm 1""],
            ""cons"": [""nhược điểm 1""],
            ""recommendations"": [""lời khuyên 1""]
          }
        }

        ### NOTE8 REVIEWS:
        {NOTE8_REVIEWS}

        ### REVIEWCONGTY REVIEWS:
        {REVIEWCONGTY_REVIEWS}";
    }
}
