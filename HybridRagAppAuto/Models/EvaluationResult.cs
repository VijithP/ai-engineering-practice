namespace HybridRagAppAuto.Models;

public class EvaluationResult
{
    public double Faithfulness { get; set; }

    public double AnswerRelevancy { get; set; }

    public double ContextPrecision { get; set; }

    public double ContextRecall { get; set; }
}