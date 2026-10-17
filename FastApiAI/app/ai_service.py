import os

from dotenv import load_dotenv
from openai import OpenAI


load_dotenv()

api_key = os.getenv("OPENAI_API_KEY")

if not api_key:
    raise RuntimeError(
        "OPENAI_API_KEY is not configured."
    )

client = OpenAI(api_key=api_key)


def generate_answer(question: str) -> str:

    response = client.responses.create(
        model="gpt-4o-mini",
        input=question
    )

    return response.output_text