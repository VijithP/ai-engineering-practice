using InternalKnowledgeAssistant.Models;

namespace InternalKnowledgeAssistant.Services;

public class CacheService
{
    private readonly Dictionary<string, CachedResponse>
        _cache = new();

    private readonly object _lock = new();

    public bool TryGet(
        string question,
        out CachedResponse? response)
    {
        var key =
            Normalize(question);

        lock (_lock)
        {
            if (_cache.TryGetValue(
                    key,
                    out var cached))
            {
                response = cached;
                return true;
            }
        }

        response = null;
        return false;
    }

    public void Set(
        string question,
        string answer,
        List<string> sources)
    {
        var key =
            Normalize(question);

        var cached =
            new CachedResponse
            {
                Question = question,
                Answer = answer,
                Sources = sources,
                CreatedAt = DateTime.UtcNow
            };

        lock (_lock)
        {
            _cache[key] = cached;
        }
    }

    private static string Normalize(
        string question)
    {
        return string.Join(
            " ",
            question
                .ToLowerInvariant()
                .Split(
                    (char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries));
    }
}