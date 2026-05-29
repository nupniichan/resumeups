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
        1. Write everything in natural, professional {LANGUAGE} (addressing the candidate as '{PRONOUN}').
        2. Keep each summary concise (2-3 sentences).
        3. Extract up to 4-5 key Pros and Cons per platform based on common themes.
        4. Provide 3-4 actionable Recommendations/Advice per platform for job seekers.
        5. DO NOT invent ratings, numbers, or details not in the raw review text.
        6. If reviews for a platform are marked ""NO_REVIEWS"", return these defaults for that platform:
           - summary: ""{NO_REVIEWS_SUMMARY}""
           - pros/cons/recommendations: [""{NO_REVIEWS_DEFAULT}""]

        ### OUTPUT FORMAT:
        {
          ""note8"": {
            ""summary"": ""..."",
            ""pros"": [""Advantage 1"", ""Advantage 2""],
            ""cons"": [""Disadvantage 1"", ""Disadvantage 2""],
            ""recommendations"": [""Recommend 1"", ""Recommend 2""]
          },
          ""reviewCongTy"": {
            ""summary"": ""..."",
            ""pros"": [""Advantage 1""],
            ""cons"": [""Disadvantage 1""],
            ""recommendations"": [""Recommend 1""]
          }
        }

        ### NOTE8 REVIEWS:
        {NOTE8_REVIEWS}

        ### REVIEWCONGTY REVIEWS:
        {REVIEWCONGTY_REVIEWS}";

        public const string IndeedSummarizationPrompt = @"Return ONLY valid JSON. Do not wrap in ```json codeblocks. No markdown, no extra text, no explanation.

        You are a senior HR consultant analyzing employee reviews from Indeed. Synthesize the provided overall rating and review titles into a professional summary, key pros/cons, and recommendations.

        ### RULES:
        1. Write everything in natural, professional {LANGUAGE} (addressing the candidate as '{PRONOUN}').
        2. Keep the summary highly professional, objective, and concise (2-3 sentences).
        3. Extract up to 4-5 key Pros and Cons of working at this company.
        4. Provide 3-4 actionable Recommendations/Advice for job seekers.
        5. DO NOT invent or include any numerical ratings in the JSON response.

        ### INPUT DATA:
        - Company: {COMPANY_NAME}
        - Overall Rating: {OVERALL_RATING} / 5 ({REVIEWS_COUNT})
        - Sample Review Titles:
        {REVIEW_TITLES}

        ### OUTPUT FORMAT:
        {
          ""summary"": ""Summary of the company's quality, culture, and employee experience."",
          ""pros"": [""Advantage 1"", ""Advantage 2""],
          ""cons"": [""Disadvantage 1"", ""Disadvantage 2""],
          ""recommendations"": [""Recommendation 1"", ""Recommendation 2""]
        }";

        public const string GlassdoorSummarizationPrompt = @"Return ONLY valid JSON. Do not wrap in ```json codeblocks. No markdown, no extra text, no explanation.

        You are a senior HR consultant analyzing a Glassdoor company review page's scraped markdown. Extract the overall rating, reviews count, category ratings, demographic group ratings, sample reviews/titles, and synthesize them into a professional summary, key pros/cons, and recommendations for job seekers.

        ### RULES:
        1. Write everything in natural, professional {LANGUAGE} (addressing the candidate as '{PRONOUN}').
        2. Extract the overall rating (0.0 to 5.0) and total reviews count.
        3. Extract or deduce category ratings (0.0 to 5.0) for:
           - Work/Life Balance (workLifeBalance)
           - Compensation & Benefits (payAndBenefits)
           - Career Opportunities (jobSecurityAndAdvancement)
           - Senior Management (management)
           - Culture & Values (culture)
           - Diversity & Inclusion (diversityAndInclusion)
        4. Extract any Ratings by Demographic Groups (e.g. by gender, department, role, location, etc.) into a list of group names and their respective rating score.
        5. Extract 5-10 recent review titles or summary lines (reviewTitles).
        6. Keep the summary highly professional, objective, and concise (2-3 sentences).
        7. Extract up to 4-5 key Pros and Cons of working at this company.
        8. Provide 3-4 actionable Recommendations/Advice for job seekers.

        ### SCRAPED GLASSDOOR MARKDOWN:
        {MARKDOWN}

        ### OUTPUT JSON FORMAT:
        {
          ""overallRating"": 4.1,
          ""reviewsCount"": ""1.2K"",
          ""workLifeBalance"": 3.8,
          ""payAndBenefits"": 4.2,
          ""jobSecurityAndAdvancement"": 3.9,
          ""management"": 3.5,
          ""culture"": 4.0,
          ""diversityAndInclusion"": 4.3,
          ""demographicRatings"": [
            { ""group"": ""Gender - Female"", ""rating"": 4.0 },
            { ""group"": ""Gender - Male"", ""rating"": 4.2 },
            { ""group"": ""Full-time Employees"", ""rating"": 4.1 }
          ],
          ""reviewTitles"": [
            ""Great work environment and smart people"",
            ""Low pay compared to market standards""
          ],
          ""summary"": ""Summary of the employee experience, culture, and general sentiment."",
          ""pros"": [""Advantage 1"", ""Advantage 2""],
          ""cons"": [""Disadvantage 1"", ""Disadvantage 2""],
          ""recommendations"": [""Recommendation 1"", ""Recommendation 2""]
        }";
    }
}
