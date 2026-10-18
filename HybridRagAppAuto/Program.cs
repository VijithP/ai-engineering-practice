using DotNetEnv;
using HybridRagAppAuto.Services;

Env.Load();

var apiKey =
    Environment.GetEnvironmentVariable(
        "OPENAI_API_KEY"
    );

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine(
        "OPENAI_API_KEY is not configured."
    );

    return;
}

var documentService =
    new DocumentService();

var documents =
    documentService.LoadDocuments();

Console.WriteLine(
    $"Loaded {documents.Count} documents."
);

var keywordSearch =
    new KeywordSearchService();

var vectorSearch =
    new VectorSearchService(apiKey);

var hybridSearch =
    new HybridSearchService(
        keywordSearch,
        vectorSearch
    );

var ragService =
    new RagService(apiKey);

Console.WriteLine();
Console.WriteLine(
    "Hybrid RAG Chatbot"
);

Console.WriteLine(
    "Type 'exit' to quit."
);

while (true)
{
    Console.WriteLine();

    Console.Write("Question: ");

    var question =
        Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
        continue;

    if (question.Equals(
        "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    Console.WriteLine();
    Console.WriteLine(
        "Searching..."
    );

    var results =
        await hybridSearch.SearchAsync(
            question,
            documents
        );

    var topResults =
        results
            .Take(3)
            .ToList();

    Console.WriteLine();
    Console.WriteLine(
        "Retrieved Documents:"
    );

    // foreach (var result in topResults)
    // {
    //     Console.WriteLine(
    //         $"{result.Document.Source} | " +
    //         $"Keyword: {result.KeywordScore:F3} | " +
    //         $"Semantic: {result.SemanticScore:F3} | " +
    //         $"Hybrid: {result.HybridScore:F3}"
    //     );
    // }

    // var context =
    //     string.Join(
    //         "\n\n",
    //         topResults.Select(
    //             x =>
    //                 $"Source: {x.Document.Source}\n" +
    //                 x.Document.Content
    //         )
    //     );

    // var answer =
    //     await ragService.GenerateAnswerAsync(
    //         question,
    //         context
    //     );

    // Console.WriteLine();
    // Console.WriteLine("Answer:");
    // Console.WriteLine(answer);


    var contexts =
    topResults
        .Select(x => x.Document.Content)
        .ToList();


    var context =
    string.Join(
        "\n\n",
        contexts
    );

    var answer =
        await ragService.GenerateAnswerAsync(
            question,
            context
        );


    var ragas = new RagasEvaluationService();
    var evaluation =
    await ragas.EvaluateAsync(
        question,
        answer,
        contexts
    );

    if (evaluation != null)
    {
        Console.WriteLine();
        Console.WriteLine("================================");
        Console.WriteLine("       RAGAS EVALUATION");
        Console.WriteLine("================================");

        Console.WriteLine(
            $"Faithfulness      : {evaluation.Faithfulness:F2}"
        );

        Console.WriteLine(
            $"Answer Relevancy  : {evaluation.AnswerRelevancy:F2}"
        );

        Console.WriteLine(
            $"Context Precision : {evaluation.ContextPrecision:F2}"
        );

        Console.WriteLine(
            $"Context Recall    : {evaluation.ContextRecall:F2}"
        );
    }
}