using OpenAI.Chat;

namespace InternalKnowledgeBase.Services;

public class RagService
{
    private readonly ChatClient _chatClient;

    public RagService(string apiKey)
    {
        _chatClient = new ChatClient(
            "gpt-4o-mini",
            apiKey
        );
    }

    public async Task<string> GenerateAnswerAsync(
        string question,
        string context)
    {
        var prompt = $"""
        You are a company policy assistant.

        Answer the user's question using ONLY the
        provided context.

        Do not invent information.

        If the answer is not available in the context,
        say that the information was not found.

        Context:
        {context}

        Question:
        {question}
        """;

        var response =
            await _chatClient.CompleteChatAsync(prompt);

        return response.Value.Content[0].Text;
    }
}