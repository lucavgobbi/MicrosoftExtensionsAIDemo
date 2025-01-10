using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace AIDemo
{
    public static class ChatClientFactory
    {
        public enum ModelsEnum
        {
            Llama31,
            Llama32,
            Qwen25,
            AzureOpenAIGpt4o,
        }
        
        public static IChatClient CreateChatClient(ModelsEnum modelId)
        {
            switch (modelId)
            {
                case ModelsEnum.Llama31:
                    return new OllamaChatClient(
                            new Uri("http://localhost:11434/"),
                            "llama3.1:8b")
                        .AsBuilder()
                        .UseFunctionInvocation()
                        .Build();
                case ModelsEnum.Llama32:
                    return new OllamaChatClient(
                            new Uri("http://localhost:11434/"),
                            "llama3.2")
                        .AsBuilder()
                        .UseFunctionInvocation()
                        .Build();
                case ModelsEnum.Qwen25:
                    return new OllamaChatClient(
                            new Uri("http://localhost:11434/"),
                            "qwen2.5:7b")
                        .AsBuilder()
                        .UseFunctionInvocation()
                        .Build();
                case ModelsEnum.AzureOpenAIGpt4o:
                    var azureOpenAiClient = new AzureOpenAIClient(
                        new Uri(Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty),
                        new System.ClientModel.ApiKeyCredential(Environment.GetEnvironmentVariable("OPENAI_KEY") ??
                                                                string.Empty)
                    );

                    return new OpenAIChatClient(
                            azureOpenAiClient,
                            "gpt-4o")
                        .AsBuilder()
                        .UseFunctionInvocation()
                        .Build();
                default:
                    throw new ArgumentException("Invalid modelId");
            }
        }
    }
}

