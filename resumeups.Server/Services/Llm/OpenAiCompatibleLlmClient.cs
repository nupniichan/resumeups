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

        using var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var requestUri = $"{_settings.BaseUrl}/chat/completions";
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
}
