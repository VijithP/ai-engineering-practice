using Microsoft.AspNetCore.Mvc;
using InternalKnowledgeBase.Services;

public class ChatController : Controller
{
    private readonly HybridSearchService _searchService;
    private readonly RagService _ragService;
    private readonly DocumentService _documentService;

    public ChatController(
        HybridSearchService searchService,
        RagService ragService,
        DocumentService documentService)
    {
        _searchService = searchService;
        _ragService = ragService;
        _documentService = documentService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Ask(
        [FromBody] ChatRequest request)
    {
        var results =
            await _searchService.SearchAsync(
                request.Question,
                _documentService.LoadDocuments()
            );

        var topResults =
            results.Take(3).ToList();

        var context =
            string.Join(
                "\n\n",
                topResults.Select(
                    x => x.Document.Content
                )
            );

        var answer =
            await _ragService.GenerateAnswerAsync(
                request.Question,
                context
            );

        return Json(new
        {
            answer,

            sources = topResults
                .Select(x => x.Document.Source)
                .Distinct()
                .ToList()
        });
    }
}