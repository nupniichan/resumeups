using System.Text;
using System.Text.Json;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Services.Llm;

public sealed class OpenAiCompatible : ILLMClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly LlmSettings _settings;

    public OpenAiCompatible(IHttpClientFactory httpClientFactory, LlmSettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }

    public async Task<string> GetResponseAsync(
        IReadOnlyList<LlmChatMessage> messages,
        double temperature = 0,
        CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = _settings.Model,
            messages = messages.Select(m => new { role = m.Role, content = m.Content }).ToArray(),
            temperature
        };

        var client = _httpClientFactory.CreateClient("llm");
        var requestUri = $"{_settings.BaseUrl}/chat/completions";
        var bodyJson = JsonSerializer.Serialize(requestBody);

        const int maxAttempts = 3;
        const int delayMs = 3000;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var jsonContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                using var response = await client.PostAsync(requestUri, jsonContent, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                using var doc = JsonDocument.Parse(responseJson);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "";
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                Console.WriteLine($"[LLM Client] Tried {attempt} and failed: {ex.Message}. Retrying in {delayMs / 1000}s...");
                await Task.Delay(delayMs, cancellationToken);
            }
        }

        throw new HttpRequestException("Can't get result from LLM.");
    }
}
