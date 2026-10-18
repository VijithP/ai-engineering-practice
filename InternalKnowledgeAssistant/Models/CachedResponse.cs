namespace InternalKnowledgeAssistant.Models;

public class CachedResponse
{
    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public List<string> Sources { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}