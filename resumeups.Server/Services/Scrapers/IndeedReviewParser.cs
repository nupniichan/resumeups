using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace resumeups.Server.Services.Scrapers
{
    public static class IndeedReviewParser
    {
        public static (string? slug, string? host) ExtractIndeedSlugAndHost(string html)
        {
            if (string.IsNullOrEmpty(html)) return (null, null);

            var match = Regex.Match(html, @"([a-z0-9.-]*indeed\.com)/cmp/([^/&?#""\s]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var host = match.Groups[1].Value;
                var slug = match.Groups[2].Value;
                return (Uri.UnescapeDataString(slug), host);
            }
            return (null, null);
        }

        public static (double? rating, string reviewsCount) ExtractRatingsFromSearchHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return (null, "");

            var matches = Regex.Matches(html, @"[a-z0-9.-]*indeed\.com/cmp/([^/&?#""\s\)\>]+)", RegexOptions.IgnoreCase);
            
            foreach (Match match in matches)
            {
                var startIndex = match.Index;
                var length = Math.Min(1200, html.Length - startIndex);
                if (length <= 0) continue;
                
                var segment = html.Substring(startIndex, length);

                var m1 = Regex.Match(segment, @"(?:Rating|Xếp hạng|Đánh giá):?\s*([1-5][.,][0-9])\s*[-–]\s*([\d,.]+k?)\s*(?:reviews|đánh giá|nhận xét|votes)", RegexOptions.IgnoreCase);
                if (m1.Success)
                {
                    var ratingVal = double.TryParse(m1.Groups[1].Value.Replace(',', '.'), out var r) ? r : (double?)null;
                    var countVal = m1.Groups[2].Value;
                    return (ratingVal, countVal);
                }

                var m2 = Regex.Match(segment, @"([1-5]\.[0-9])\s*/\s*5", RegexOptions.IgnoreCase);
                if (m2.Success)
                {
                    var ratingVal = double.TryParse(m2.Groups[1].Value, out var r) ? r : (double?)null;
                    var mCount = Regex.Match(segment, @"([\d,.]+k?)\s*(?:reviews|đánh giá|nhận xét|votes)", RegexOptions.IgnoreCase);
                    var countVal = mCount.Success ? mCount.Groups[1].Value : "";
                    return (ratingVal, countVal);
                }

                var m3 = Regex.Match(segment, @"([1-5][.,][0-9])\s*(?:★|stars?|out of 5)", RegexOptions.IgnoreCase);
                if (m3.Success)
                {
                    var ratingVal = double.TryParse(m3.Groups[1].Value.Replace(',', '.'), out var r) ? r : (double?)null;
                    var mCount = Regex.Match(segment, @"([\d,.]+k?)\s*(?:reviews|đánh giá|nhận xét|votes)", RegexOptions.IgnoreCase);
                    var countVal = mCount.Success ? mCount.Groups[1].Value : "";
                    return (ratingVal, countVal);
                }
            }

            return (null, "");
        }

        public static (double? overallRating, string reviewsCount) ParseOverallRatingAndCount(string markdown)
        {
            double? overallRating = null;
            string reviewsCount = "";

            if (string.IsNullOrEmpty(markdown)) return (null, "");

            var ratingMatch = Regex.Match(markdown, @"([1-5]\.[0-9])\s*(?:out of 5|star|★)", RegexOptions.IgnoreCase);
            if (ratingMatch.Success && double.TryParse(ratingMatch.Groups[1].Value, out var r1))
            {
                overallRating = r1;
            }
            else
            {
                var ratingMatch2 = Regex.Match(markdown, @"\b([1-5]\.[0-9])\b", RegexOptions.IgnoreCase);
                if (ratingMatch2.Success && double.TryParse(ratingMatch2.Groups[1].Value, out var r2))
                {
                    overallRating = r2;
                }
            }

            var countMatch = Regex.Match(markdown, @"([\d,.]+K?)\s*(?:reviews|đánh giá|ratings)", RegexOptions.IgnoreCase);
            if (countMatch.Success)
            {
                reviewsCount = countMatch.Groups[1].Value;
            }

            return (overallRating, reviewsCount);
        }

        public static double? ParseCategoryScore(string markdown, string[] keywords)
        {
            if (string.IsNullOrEmpty(markdown)) return null;

            foreach (var keyword in keywords)
            {
                var patternForward = $@"{Regex.Escape(keyword)}[\s\S]{{0,50}}?([1-5]\.[0-9])";
                var matchF = Regex.Match(markdown, patternForward, RegexOptions.IgnoreCase);
                if (matchF.Success && double.TryParse(matchF.Groups[1].Value, out var valF))
                    return valF;

                var patternBackward = $@"([1-5]\.[0-9])[\s\S]{{0,50}}?{Regex.Escape(keyword)}";
                var matchB = Regex.Match(markdown, patternBackward, RegexOptions.IgnoreCase);
                if (matchB.Success && double.TryParse(matchB.Groups[1].Value, out var valB))
                    return valB;
            }
            return null;
        }

        public static List<string> ParseReviewTitles(string markdown)
        {
            var titles = new List<string>();

            if (string.IsNullOrEmpty(markdown)) return titles;

            var patternWhatPeopleSay = @"(?:What people are saying|What people like|Những gì mọi người đang nói)[\s\S]{0,300}?((?:[-*+]\s+[^\n]+\n?)+)";
            var matchSaying = Regex.Match(markdown, patternWhatPeopleSay, RegexOptions.IgnoreCase);
            if (matchSaying.Success)
            {
                var listContent = matchSaying.Groups[1].Value;
                var listItems = Regex.Matches(listContent, @"[-*+]\s+([^\n\r]+)");
                foreach (Match item in listItems)
                {
                    var title = item.Groups[1].Value.Trim().Trim('"').Trim('\'').Trim('“').Trim('”');
                    if (IsValidTitle(title))
                    {
                        titles.Add(title);
                    }
                }
            }

            if (titles.Count < 3)
            {
                var listMatches = Regex.Matches(markdown, @"^[-*+]\s+[""''“]?(.*?)[""''”]??$", RegexOptions.Multiline);
                foreach (Match match in listMatches)
                {
                    var title = match.Groups[1].Value.Trim().Trim('"').Trim('\'').Trim('“').Trim('”');
                    if (IsValidTitle(title) && !titles.Contains(title))
                    {
                        titles.Add(title);
                    }
                }

                var headingMatches = Regex.Matches(markdown, @"^#{2,4}\s+[""''“]?(.*?)[""''”]??$", RegexOptions.Multiline);
                foreach (Match match in headingMatches)
                {
                    var title = match.Groups[1].Value.Trim().Trim('"').Trim('\'').Trim('“').Trim('”');
                    if (IsValidTitle(title) && !titles.Contains(title))
                    {
                        titles.Add(title);
                    }
                }

                var boldMatches = Regex.Matches(markdown, @"\*\*[""''“]?(.*?)[""''”]??\*\*", RegexOptions.IgnoreCase);
                foreach (Match match in boldMatches)
                {
                    var title = match.Groups[1].Value.Trim().Trim('"').Trim('\'').Trim('“').Trim('”');
                    if (IsValidTitle(title) && !titles.Contains(title))
                    {
                        titles.Add(title);
                    }
                }
            }

            return titles.Distinct().Take(10).ToList();
        }

        public static bool IsValidTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            if (title.Length < 3 || title.Length > 120) return false;

            var lowercase = title.ToLower();
            string[] blacklistedKeywords = {
                "indeed", "review", "work", "rating", "find jobs", "company review", 
                "salaries", "questions", "photos", "jobs", "sign in", "post a job", 
                "employer", "follow", "about", "claim this company", "overview", 
                "ratings by category", "sort by", "filter by", "working at", 
                "vietnamese", "english", "all countries", "all languages"
            };

            foreach (var keyword in blacklistedKeywords)
            {
                if (lowercase.Contains(keyword) && lowercase.Length < 25)
                    return false;
            }

            return true;
        }
    }
}
