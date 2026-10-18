using System.Text.Json;
using InternalKnowledgeAssistant.Models;

namespace InternalKnowledgeAssistant.Services;

public class VectorStoreService
{
    private readonly string _filePath;

    private readonly JsonSerializerOptions _jsonOptions =
        new()
        {
            WriteIndented = true
        };

    public VectorStoreService(
        IWebHostEnvironment environment)
    {
        var dataDirectory =
            Path.Combine(
                environment.ContentRootPath,
                "Data");

        Directory.CreateDirectory(dataDirectory);

        _filePath =
            Path.Combine(
                dataDirectory,
                "VectorStore.json");
    }

    public async Task SaveAsync(
        List<DocumentChunk> chunks)
    {
        var json =
            JsonSerializer.Serialize(
                chunks,
                _jsonOptions);

        await File.WriteAllTextAsync(
            _filePath,
            json);
    }

    public async Task<List<DocumentChunk>> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<DocumentChunk>();
        }

        var json =
            await File.ReadAllTextAsync(
                _filePath);

        return JsonSerializer.Deserialize<
                   List<DocumentChunk>>(
                   json)
               ?? new List<DocumentChunk>();
    }
}