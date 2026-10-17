public class CostCalculator
{
    public static decimal CalculateCost(
        int requests,
        int inputTokensPerRequest,
        int outputTokensPerRequest,
        decimal inputPricePerMillion,
        decimal outputPricePerMillion)
    {
        decimal totalInputTokens =
            requests * inputTokensPerRequest;

        decimal totalOutputTokens =
            requests * outputTokensPerRequest;

        decimal inputCost =
            (totalInputTokens / 1_000_000m)
            * inputPricePerMillion;

        decimal outputCost =
            (totalOutputTokens / 1_000_000m)
            * outputPricePerMillion;

        return inputCost + outputCost;
    }
}