using resumeups.Server.Utils;

namespace resumeups.Server.Models;

public sealed class LlmSettings
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
    public required string Model { get; init; }

    public static LlmSettings FromEnvironment()
    {
        var baseUrl = RequireEnv("LLM_BASE_URL").TrimEnd('/');
        var apiKey = RequireEnv("LLM_API_KEY");
        var model = RequireEnv("LLM_MODEL");

        return new LlmSettings
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            Model = model
        };
    }

    private static string RequireEnv(string key)
    {
        var value = EnvReader.Get(key).Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{key} is required in .env");

        return value;
    }
}
