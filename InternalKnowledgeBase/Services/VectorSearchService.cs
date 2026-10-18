using InternalKnowledgeBase.Models;
using OpenAI.Embeddings;

namespace InternalKnowledgeBase.Services;

public class VectorSearchService
{
    private readonly EmbeddingClient _embeddingClient;

    public VectorSearchService(string apiKey)
    {
        _embeddingClient = new EmbeddingClient(
            "text-embedding-3-small",
            apiKey
        );
    }

    public async Task<List<SearchResult>> SearchAsync(
        string query,
        List<DocumentChunk> documents)
    {
        var queryEmbedding =
            await _embeddingClient.GenerateEmbeddingAsync(query);

        var queryVector =
            queryEmbedding.Value.ToFloats().ToArray();

        var results = new List<SearchResult>();

        foreach (var document in documents)
        {
            if (document.Embedding == null)
            {
                var embedding =
                    await _embeddingClient.GenerateEmbeddingAsync(
                        document.Content
                    );

                document.Embedding =
                    embedding.Value.ToFloats().ToArray();
            }

            var score = CosineSimilarity(
                queryVector,
                document.Embedding
            );

            results.Add(
                new SearchResult
                {
                    Document = document,
                    SemanticScore = score
                }
            );
        }

        return results
            .OrderByDescending(x => x.SemanticScore)
            .ToList();
    }

    private static double CosineSimilarity(
        float[] vectorA,
        float[] vectorB)
    {
        double dotProduct = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (var i = 0; i < vectorA.Length; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];

            magnitudeA +=
                vectorA[i] * vectorA[i];

            magnitudeB +=
                vectorB[i] * vectorB[i];
        }

        if (magnitudeA == 0 || magnitudeB == 0)
            return 0;

        return dotProduct /
               (Math.Sqrt(magnitudeA) *
                Math.Sqrt(magnitudeB));
    }
}