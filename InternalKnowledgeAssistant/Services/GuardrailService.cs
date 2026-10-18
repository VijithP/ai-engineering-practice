namespace InternalKnowledgeAssistant.Services;

public class GuardrailService
{
    private readonly string[] _blockedPatterns =
    {
        "ignore previous instructions",
        "ignore all previous instructions",
        "system prompt",
        "reveal your prompt",
        "show your instructions",
        "api key",
        "password",
        "secret key"
    };

    public bool ValidateInput(
        string question,
        out string error)
    {
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(question))
        {
            error = "Question cannot be empty.";
            return false;
        }

        if (question.Length > 1000)
        {
            error =
                "Question is too long. Maximum length is 1000 characters.";

            return false;
        }

        var lower =
            question.ToLowerInvariant();

        foreach (var pattern in _blockedPatterns)
        {
            if (lower.Contains(pattern))
            {
                error =
                    "The request was blocked by the input guardrail.";

                return false;
            }
        }

        return true;
    }

    public bool ValidateOutput(
        string answer,
        out string error)
    {
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(answer))
        {
            error =
                "The AI generated an empty response.";

            return false;
        }

        if (answer.Length > 10000)
        {
            error =
                "The AI response exceeded the allowed length.";

            return false;
        }

        if (answer.Contains("sk-"))
        {
            error =
                "Potential API key detected in output.";

            return false;
        }

        return true;
    }
}