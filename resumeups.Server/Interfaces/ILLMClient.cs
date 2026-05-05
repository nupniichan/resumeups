using resumeups.Server.Models;

namespace resumeups.Server.Interfaces;

public interface ILLMClient
{
    Task<string> GetResponseAsync(
        IReadOnlyList<LlmChatMessage> messages,
        double temperature = 0,
        CancellationToken cancellationToken = default);
}
