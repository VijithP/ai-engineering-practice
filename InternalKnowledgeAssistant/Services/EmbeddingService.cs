using OpenAI.Embeddings;

namespace InternalKnowledgeAssistant.Services;

public class EmbeddingService
{
    private readonly EmbeddingClient _client;

    public EmbeddingService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"]
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? throw new InvalidOperationException(
                "OPENAI_API_KEY is not configured.");

        var model = configuration["OpenAI:EmbeddingModel"]
            ?? "text-embedding-3-small";

        _client = new EmbeddingClient(
            model,
            apiKey);
    }

    public async Task<float[]> CreateEmbeddingAsync(
        string text)
    {
        var result =
            await _client.GenerateEmbeddingAsync(text);

        return result.Value.ToFloats().ToArray();
    }
}