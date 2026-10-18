public class SearchResult
{
    public DocumentChunk Document { get; set; } = null!;

    public double KeywordScore { get; set; }

    public double SemanticScore { get; set; }

    public double HybridScore { get; set; }
}