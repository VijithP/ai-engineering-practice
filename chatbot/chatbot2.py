from openai import OpenAI
from dotenv import load_dotenv
import os

import openai

load_dotenv()

client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

messages = [
    {"role": "user", "content": "Hello"}
]

response = client.chat.completions.create(
    model="gpt-4.1-mini",
    messages=messages
)

print(response.choices[0].message.content)