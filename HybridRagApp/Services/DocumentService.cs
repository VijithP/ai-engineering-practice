using HybridRagApp.Models;

namespace HybridRagApp.Services;

public class DocumentService
{
    public List<DocumentChunk> LoadDocuments()
    {
        var documents = new List<DocumentChunk>();

        var dataPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data"
        );

        var files = Directory.GetFiles(
            dataPath,
            "*.txt"
        );

        var id = 1;

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);

            documents.Add(
                new DocumentChunk
                {
                    Id = id++,
                    Content = content,
                    Source = Path.GetFileName(file)
                }
            );
        }

        return documents;
    }
}