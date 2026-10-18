using InternalKnowledgeAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalKnowledgeAssistant.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStore;

    public AdminController(
        DocumentService documentService,
        EmbeddingService embeddingService,
        VectorStoreService vectorStore)
    {
        _documentService = documentService;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest()
    {
        var chunks =
            _documentService
                .LoadAndChunkDocuments();

        foreach (var chunk in chunks)
        {
            chunk.Embedding =
                await _embeddingService
                    .CreateEmbeddingAsync(
                        chunk.Content);
        }

        await _vectorStore.SaveAsync(
            chunks);

        return Ok(
            new
            {
                message =
                    "Documents indexed successfully.",

                documents =
                    chunks
                        .Select(x => x.DocumentName)
                        .Distinct()
                        .Count(),

                chunks =
                    chunks.Count
            });
    }
}