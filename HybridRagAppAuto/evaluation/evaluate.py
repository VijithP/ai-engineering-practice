import sys
import json
import asyncio

from ragas import EvaluationDataset, evaluate
from ragas.metrics import (
    Faithfulness,
    ResponseRelevancy,
    LLMContextPrecisionWithoutReference,
    LLMContextRecall
)


async def main():

    input_data = json.loads(
        sys.stdin.read()
    )

    question = input_data["question"]
    answer = input_data["answer"]
    contexts = input_data["contexts"]
    reference_answer = input_data.get(
        "reference_answer"
    )

    sample = {
        "user_input": question,
        "response": answer,
        "retrieved_contexts": contexts
    }

    if reference_answer:
        sample["reference"] = reference_answer

    dataset = EvaluationDataset.from_list([sample])

    metrics = [
        Faithfulness(),
        ResponseRelevancy(),
        LLMContextPrecisionWithoutReference()
    ]

    if reference_answer:
        metrics.append(LLMContextRecall())

    result = evaluate(
        dataset=dataset,
        metrics=metrics
    )

    scores = result.to_pandas().iloc[0]

    response = {
        "faithfulness": float(
            scores.get("faithfulness", 0)
        ),

        "answerRelevancy": float(
            scores.get("response_relevancy", 0)
        ),

        "contextPrecision": float(
            scores.get("context_precision", 0)
        ),

        "contextRecall": float(
            scores.get("context_recall", 0)
        )
    }

    print(
        json.dumps(response)
    )


if __name__ == "__main__":
    asyncio.run(main())