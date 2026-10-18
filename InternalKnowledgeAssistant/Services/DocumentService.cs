using InternalKnowledgeAssistant.Models;

namespace InternalKnowledgeAssistant.Services;

public class DocumentService
{
    private readonly string _documentsPath;

    public DocumentService(IWebHostEnvironment environment)
    {
        _documentsPath = Path.Combine(environment.ContentRootPath, "Documents");
    }

    public List<(string Name, string Content)> LoadDocuments()
    {
        var documents = new List<(string Name, string Content)>();

        if (!Directory.Exists(_documentsPath))
        {
            Directory.CreateDirectory(_documentsPath);
            return documents;
        }

        foreach (var file in Directory.GetFiles(_documentsPath, "*.txt"))
        {
            var content = File.ReadAllText(file);

            if (!string.IsNullOrWhiteSpace(content))
            {
                documents.Add(
                    (
                        Path.GetFileName(file),
                        content
                    ));
            }
        }

        return documents;
    }

    public List<DocumentChunk> CreateChunks(
        string documentName,
        string content,
        int chunkSize = 800)
    {
        var chunks = new List<DocumentChunk>();

        var words = content
            .Split(
                new[] { ' ', '\r', '\n', '\t' },
                StringSplitOptions.RemoveEmptyEntries);

        var current = new List<string>();
        var chunkIndex = 0;

        foreach (var word in words)
        {
            current.Add(word);

            if (current.Count >= chunkSize)
            {
                chunks.Add(new DocumentChunk
                {
                    Id = Guid.NewGuid().ToString(),
                    DocumentName = documentName,
                    ChunkIndex = chunkIndex++,
                    Content = string.Join(" ", current)
                });

                current.Clear();
            }
        }

        if (current.Count > 0)
        {
            chunks.Add(new DocumentChunk
            {
                Id = Guid.NewGuid().ToString(),
                DocumentName = documentName,
                ChunkIndex = chunkIndex,
                Content = string.Join(" ", current)
            });
        }

        return chunks;
    }

    public List<DocumentChunk> LoadAndChunkDocuments()
    {
        var result = new List<DocumentChunk>();

        foreach (var document in LoadDocuments())
        {
            result.AddRange(
                CreateChunks(
                    document.Name,
                    document.Content));
        }

        return result;
    }
}