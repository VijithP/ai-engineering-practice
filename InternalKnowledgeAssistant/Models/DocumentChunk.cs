namespace InternalKnowledgeAssistant.Models;

public class DocumentChunk
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string DocumentName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int ChunkIndex { get; set; }

    public float[] Embedding { get; set; } = Array.Empty<float>();
}