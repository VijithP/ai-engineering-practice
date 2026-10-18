using HybridRagApp.Models;

namespace HybridRagApp.Services;

public class HybridSearchService
{
    private readonly KeywordSearchService _keywordSearch;
    private readonly VectorSearchService _vectorSearch;

    public HybridSearchService(
        KeywordSearchService keywordSearch,
        VectorSearchService vectorSearch)
    {
        _keywordSearch = keywordSearch;
        _vectorSearch = vectorSearch;
    }

    public async Task<List<SearchResult>> SearchAsync(
        string query,
        List<DocumentChunk> documents)
    {
        var keywordResults =
            _keywordSearch.Search(
                query,
                documents
            );

        var semanticResults =
            await _vectorSearch.SearchAsync(
                query,
                documents
            );

        var keywordScores =
            keywordResults.ToDictionary(
                x => x.Document.Id,
                x => x.KeywordScore
            );

        var semanticScores =
            semanticResults.ToDictionary(
                x => x.Document.Id,
                x => x.SemanticScore
            );

        var maxKeyword =
            keywordScores.Values.DefaultIfEmpty(1).Max();

        var results = new List<SearchResult>();

        foreach (var document in documents)
        {
            var keywordScore =
                keywordScores.GetValueOrDefault(
                    document.Id
                );

            var semanticScore =
                semanticScores.GetValueOrDefault(
                    document.Id
                );

            // Normalize keyword score
            var normalizedKeyword =
                maxKeyword > 0
                    ? keywordScore / maxKeyword
                    : 0;

            var hybridScore =
                (0.4 * normalizedKeyword) +
                (0.6 * semanticScore);

            results.Add(
                new SearchResult
                {
                    Document = document,
                    KeywordScore = normalizedKeyword,
                    SemanticScore = semanticScore,
                    HybridScore = hybridScore
                }
            );
        }

        return results
            .OrderByDescending(x => x.HybridScore)
            .ToList();
    }
}