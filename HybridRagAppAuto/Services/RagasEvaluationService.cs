using System.Diagnostics;
using System.Text;
using System.Text.Json;
using HybridRagAppAuto.Models;

namespace HybridRagAppAuto.Services;

public class RagasEvaluationService
{
    private readonly string _pythonExecutable;
    private readonly string _evaluationScript;

    public RagasEvaluationService()
    {
        _pythonExecutable = "python";

        _evaluationScript = Path.Combine(
            Directory.GetCurrentDirectory(),
            "evaluation",
            "evaluate.py"
        );
    }

    public async Task<EvaluationResult?> EvaluateAsync(
        string question,
        string answer,
        List<string> contexts,
        string? referenceAnswer = null)
    {
        var request = new
        {
            question,
            answer,
            contexts,
            reference_answer = referenceAnswer
        };

        var json = JsonSerializer.Serialize(request);

        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonExecutable,
            Arguments = $"\"{_evaluationScript}\"",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process
        {
            StartInfo = startInfo
        };

        process.Start();

        await process.StandardInput.WriteAsync(json);
        process.StandardInput.Close();

        var output =
            await process.StandardOutput.ReadToEndAsync();

        var error =
            await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            Console.WriteLine(
                $"RAGAS evaluation failed: {error}"
            );

            return null;
        }

        return JsonSerializer.Deserialize<EvaluationResult>(
            output,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
    }
}