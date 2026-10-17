from fastapi import FastAPI, HTTPException

from app.models import AskRequest, AskResponse
from app.ai_service import generate_answer


app = FastAPI(
    title="AI Feature Microservice",
    description="AI Q&A microservice using FastAPI and OpenAI",
    version="1.0.0"
)


@app.get("/")
def root():

    return {
        "message": "AI Feature Microservice is running"
    }


@app.get("/health")
def health():

    return {
        "status": "healthy"
    }


@app.post(
    "/api/ai/ask",
    response_model=AskResponse
)
def ask(request: AskRequest):

    try:

        answer = generate_answer(
            request.question
        )

        return AskResponse(
            answer=answer
        )

    except Exception as ex:

        raise HTTPException(
            status_code=500,
            detail=str(ex)
        )