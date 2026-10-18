using HybridRagAppAuto.Models;

namespace HybridRagAppAuto.Services;

public class KeywordSearchService
{
    public List<SearchResult> Search(
        string query,
        List<DocumentChunk> documents)
    {
        var queryTerms = Tokenize(query);

        var results = new List<SearchResult>();

        foreach (var document in documents)
        {
            var documentTerms = Tokenize(document.Content);

            double score = 0;

            foreach (var term in queryTerms)
            {
                var frequency = documentTerms.Count(
                    x => x == term
                );

                score += frequency;
            }

            results.Add(
                new SearchResult
                {
                    Document = document,
                    KeywordScore = score
                }
            );
        }

        return results
            .OrderByDescending(x => x.KeywordScore)
            .ToList();
    }

    private static List<string> Tokenize(string text)
    {
        return text
            .ToLowerInvariant()
            .Split(
                new[]
                {
                    ' ',
                    '\n',
                    '\r',
                    '.',
                    ',',
                    ':',
                    ';',
                    '(',
                    ')'
                },
                StringSplitOptions.RemoveEmptyEntries
            )
            .ToList();
    }
}