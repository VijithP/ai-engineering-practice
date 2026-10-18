namespace InternalKnowledgeAssistant.Models;

public class SearchResult
{
    public string DocumentName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public double SemanticScore { get; set; }

    public double KeywordScore { get; set; }

    public double FinalScore { get; set; }

    public int ChunkIndex { get; set; }
}