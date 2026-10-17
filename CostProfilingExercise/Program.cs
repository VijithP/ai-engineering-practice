Console.WriteLine(
    "====================================");

Console.WriteLine(
    "       LLM Cost Profiling");

Console.WriteLine(
    "====================================");

int requests = 1000;

int inputTokensPerRequest = 1000;

int outputTokensPerRequest = 500;

// Enter the current model pricing here.
decimal inputPricePerMillion = 1.00m;

decimal outputPricePerMillion = 2.00m;

decimal totalCost =
    CostCalculator.CalculateCost(
        requests,
        inputTokensPerRequest,
        outputTokensPerRequest,
        inputPricePerMillion,
        outputPricePerMillion);

Console.WriteLine();

Console.WriteLine(
    $"Requests: {requests}");

Console.WriteLine(
    $"Input tokens/request: {inputTokensPerRequest}");

Console.WriteLine(
    $"Output tokens/request: {outputTokensPerRequest}");

Console.WriteLine();

Console.WriteLine(
    $"Total cost: ${totalCost:F4}");

Console.WriteLine(
    $"Cost/request: ${totalCost / requests:F6}");