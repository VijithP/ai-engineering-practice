using InternalKnowledgeAssistant.Models;

namespace InternalKnowledgeAssistant.Services;

public class HybridSearchService
{
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStore;

    public HybridSearchService(
        EmbeddingService embeddingService,
        VectorStoreService vectorStore)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }

    public async Task<List<SearchResult>> SearchAsync(
        string query,
        int topK = 5)
    {
        var chunks =
            await _vectorStore.LoadAsync();

        if (chunks.Count == 0)
        {
            return new List<SearchResult>();
        }

        var queryEmbedding =
            await _embeddingService.CreateEmbeddingAsync(
                query);

        var queryWords =
            Tokenize(query);

        var results = new List<SearchResult>();

        foreach (var chunk in chunks)
        {
            var semanticScore =
                CosineSimilarity(
                    queryEmbedding,
                    chunk.Embedding);

            var keywordScore =
                KeywordSimilarity(
                    queryWords,
                    chunk.Content);

            var finalScore =
                (semanticScore * 0.7)
                +
                (keywordScore * 0.3);

            results.Add(new SearchResult
            {
                DocumentName = chunk.DocumentName,
                Content = chunk.Content,
                ChunkIndex = chunk.ChunkIndex,
                SemanticScore = semanticScore,
                KeywordScore = keywordScore,
                FinalScore = finalScore
            });
        }

        return results
            .OrderByDescending(x => x.FinalScore)
            .Take(topK)
            .ToList();
    }

    private static string[] Tokenize(
        string text)
    {
        return text
            .ToLowerInvariant()
            .Split(
                new[]
                {
                    ' ',
                    '.',
                    ',',
                    '?',
                    '!',
                    ':',
                    ';',
                    '\r',
                    '\n',
                    '\t'
                },
                StringSplitOptions.RemoveEmptyEntries);
    }

    private static double KeywordSimilarity(
        string[] queryWords,
        string content)
    {
        if (queryWords.Length == 0)
        {
            return 0;
        }

        var contentWords =
            Tokenize(content)
                .ToHashSet();

        var matches =
            queryWords
                .Distinct()
                .Count(
                    word =>
                        contentWords.Contains(word));

        return
            (double)matches /
            queryWords.Distinct().Count();
    }

    private static double CosineSimilarity(
        float[] vectorA,
        float[] vectorB)
    {
        if (vectorA.Length == 0 ||
            vectorB.Length == 0 ||
            vectorA.Length != vectorB.Length)
        {
            return 0;
        }

        double dot = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (var i = 0; i < vectorA.Length; i++)
        {
            dot +=
                vectorA[i] *
                vectorB[i];

            magnitudeA +=
                vectorA[i] *
                vectorA[i];

            magnitudeB +=
                vectorB[i] *
                vectorB[i];
        }

        if (magnitudeA == 0 ||
            magnitudeB == 0)
        {
            return 0;
        }

        return
            dot /
            (
                Math.Sqrt(magnitudeA)
                *
                Math.Sqrt(magnitudeB)
            );
    }
}