public class EvaluationResult
{
    public int Id { get; set; }

    public string Question { get; set; }
        = string.Empty;

    public string Answer { get; set; }
        = string.Empty;

    public double Faithfulness { get; set; }

    public double AnswerRelevancy { get; set; }

    public double ContextPrecision { get; set; }

    public double ContextRecall { get; set; }

    public DateTime EvaluatedAt { get; set; }
}