using DotNetEnv;

Env.Load();

var guardrails =
    new GuardrailService();

var aiService =
    new AiService(guardrails);

Console.WriteLine(
    "================================");

Console.WriteLine(
    "     AI Guardrails Demo");

Console.WriteLine(
    "================================");

Console.WriteLine();

while (true)
{
    Console.Write("Question: ");

    var question = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
    {
        continue;
    }

    if (question.Equals(
        "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        var answer =
            await aiService.AskAsync(
                question);

        Console.WriteLine();
        Console.WriteLine(
            $"AI: {answer}");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"Blocked: {ex.Message}");
        Console.WriteLine();
    }
}