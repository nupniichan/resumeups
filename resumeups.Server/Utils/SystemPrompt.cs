public class SystemPrompt
{
    public static string MatchingSystemPrompt => _matching;
    public static string FeedbackSystemPrompt => _feedback;

    private const string _matching = @"Return ONLY valid JSON. Do not wrap in ```json codeblocks.
    You are a Senior TA. Extract all unique keywords from [JD] (hard skills, tools, frameworks, certs, education, domain knowledge). 
    Normalize aliases (e.g., JS→JavaScript, K8s→Kubernetes, AWS→Amazon Web Services). 
    Compare the normalized JD keywords against the [RESUME]. No partial credit for skills.

    Output format:
    {
      ""keywords_matching"": [""keyword1"", ""keyword2""],
      ""keywords_missing"": [""keyword3"", ""keyword4""]
    }

    [RESUME]: {RESUME}
    [JD]: {JD}";

    private const string _feedback = @"Return ONLY valid JSON. Do not wrap in ```json codeblocks.
    You are a Senior TA reviewing a resume against a job description. Ignore all the personal contact information that already masked, focus on the main idea only.
    Given this keyword audit: match_score is {MATCH_SCORE}%, matched keywords: {KEYWORDS_MATCHING}, missing keywords: {KEYWORDS_MISSING}.

    Evaluate the resume and provide scores (0-100):
    - context_score: 90+=concrete achievements, 70+=partial context, 50+=skills-only, <50=disconnected.
    - impact_score: 85+=strong verbs+metrics, 60+=good structure no metrics, 40+=vague, <40=poor.

    Find resume issues. Category must be one of: [Keyword Gap, Context & Domain, Impact & Metrics, ATS Compliance]. Severity: [Critical, Optimization]. 
    Write feedback in English, addressing the user as ""you"". Provide specific Before and After text for suggestions and what already good, what need to be improved.

    Output format:
    {
      ""context_score"": <int>,
      ""impact_score"": <int>,
      ""summary"": ""<2-3 sentences overall evaluation>"",
      ""issues"": [
        {
          ""category"": ""<Category>"",
          ""severity"": ""<Severity>"",
          ""issue"": ""<Description of issue>"",
          ""before"": ""<Original resume text>"",
          ""after"": ""<Improved resume text>""
        }
      ],
      ""suggestions"": [
        {
          ""severity"": ""<Severity>"",
          ""suggestion"": ""<Description of suggestion>""
        }
      ]
    }

    [RESUME]: {RESUME}
    [JD]: {JD}";
}