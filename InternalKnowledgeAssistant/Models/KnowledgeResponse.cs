namespace InternalKnowledgeAssistant.Models;

public class KnowledgeResponse
{
    public string Answer { get; set; } = string.Empty;

    public bool Cached { get; set; }

    public List<string> Sources { get; set; } = new();

    public int RetrievedChunks { get; set; }

    public int InputTokens { get; set; }

    public int OutputTokens { get; set; }

    public double EstimatedCost { get; set; }
}