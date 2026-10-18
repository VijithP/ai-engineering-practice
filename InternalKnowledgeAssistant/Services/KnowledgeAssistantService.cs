using InternalKnowledgeAssistant.Models;

namespace InternalKnowledgeAssistant.Services;

public class KnowledgeAssistantService
{
    private readonly HybridSearchService _searchService;
    private readonly OpenAiService _openAiService;
    private readonly GuardrailService _guardrailService;
    private readonly CacheService _cacheService;

    public KnowledgeAssistantService(
        HybridSearchService searchService,
        OpenAiService openAiService,
        GuardrailService guardrailService,
        CacheService cacheService)
    {
        _searchService = searchService;
        _openAiService = openAiService;
        _guardrailService = guardrailService;
        _cacheService = cacheService;
    }

    public async Task<KnowledgeResponse> AskAsync(
        string question)
    {
        if (!_guardrailService.ValidateInput(
                question,
                out var inputError))
        {
            throw new ArgumentException(
                inputError);
        }

        if (_cacheService.TryGet(
                question,
                out var cached))
        {
            return new KnowledgeResponse
            {
                Answer = cached!.Answer,
                Cached = true,
                Sources = cached.Sources
            };
        }

        var searchResults =
            await _searchService.SearchAsync(
                question,
                topK: 5);

        if (searchResults.Count == 0)
        {
            return new KnowledgeResponse
            {
                Answer =
                    "I couldn't find any relevant information in the internal knowledge base.",
                Cached = false
            };
        }

        var context =
            string.Join(
                "\n\n--- SOURCE ---\n\n",
                searchResults.Select(
                    x =>
                        $"Document: {x.DocumentName}\n" +
                        $"Content: {x.Content}"));

        var answer =
            await _openAiService.GenerateAnswerAsync(
                question,
                context);

        if (!_guardrailService.ValidateOutput(
                answer,
                out var outputError))
        {
            throw new InvalidOperationException(
                outputError);
        }

        var sources =
            searchResults
                .Select(x => x.DocumentName)
                .Distinct()
                .ToList();

        _cacheService.Set(
            question,
            answer,
            sources);

        return new KnowledgeResponse
        {
            Answer = answer,
            Cached = false,
            Sources = sources,
            RetrievedChunks = searchResults.Count,

            // Approximate values for assignment/demo.
            InputTokens =
                EstimateTokens(context + question),

            OutputTokens =
                EstimateTokens(answer),

            EstimatedCost =
                CalculateEstimatedCost(
                    context + question,
                    answer)
        };
    }

    private static int EstimateTokens(
        string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        return Math.Max(
            1,
            text.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
                .Length * 4 / 3);
    }

    private static double CalculateEstimatedCost(
        string input,
        string output)
    {
        var inputTokens =
            EstimateTokens(input);

        var outputTokens =
            EstimateTokens(output);

        // Illustrative pricing for assignment/demo.
        // Replace with the current model pricing
        // before production use.

        const double inputPricePerMillion = 2.0;
        const double outputPricePerMillion = 8.0;

        return
            (
                inputTokens /
                1_000_000.0 *
                inputPricePerMillion
            )
            +
            (
                outputTokens /
                1_000_000.0 *
                outputPricePerMillion
            );
    }
}