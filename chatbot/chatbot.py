import os
import openai
from dotenv import load_dotenv, find_dotenv

# Load API Key
_ = load_dotenv(find_dotenv())

openai.api_key = os.getenv("OPENAI_API_KEY")


def get_completion_from_messages(messages,
                                 model="gpt-3.5-turbo",
                                 temperature=0.7):

    response = openai.ChatCompletion.create(
        model=model,
        messages=messages,
        temperature=temperature
    )

    return response.choices[0].message["content"]


messages = [
    {
        "role": "system",
        "content":
        """
        You are a friendly Employee Information Chatbot.

        Your job is to:
        1. Ask the employee for:
            - Name
            - Address
            - Skills

        2. Remember the information during the conversation.

        3. If any information is missing, politely ask for it.

        4. When all information is collected,
           display a summary.

        5. Answer questions like:
            - What is my name?
            - What is my address?
            - What are my skills?
        """
    }
]

print("====================================")
print("Employee Information Chatbot")
print("Type 'exit' to quit")
print("====================================")

while True:

    user_input = input("\nYou: ")

    if user_input.lower() == "exit":
        print("Goodbye!")
        break

    messages.append(
        {
            "role": "user",
            "content": user_input
        }
    )

    response = get_completion_from_messages(messages)

    print("\nBot:", response)

    messages.append(
        {
            "role": "assistant",
            "content": response
        }
    )