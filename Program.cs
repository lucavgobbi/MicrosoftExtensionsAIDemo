using AIDemo;
using Microsoft.Extensions.AI;

// You can toggle the features of the demo by changing the values of the following variables
var imageDemo = false;
var toolsDemo = false;
var ragDemo = false;

// You can change the model being used by changing the parameter of the CreateChatClient method
var client = ChatClientFactory.CreateChatClient(ChatClientFactory.ModelsEnum.Qwen25);

var messages = new List<ChatMessage>();

# region "Rag Setup"

var ragProvider = new OllamaRag();

# endregion

# region "Tool Setup"

var tools = new AIDemo.Tools(3);

var chatOptions = new ChatOptions
{
    ToolMode = ChatToolMode.Auto,
    Tools = tools.GetTools(),
};

# endregion

# region "Image Demo"
if (imageDemo)
{
    var messageWithImage = new ChatMessage
    {
        Role = ChatRole.User,
        Contents = new List<AIContent>()
        {
            new TextContent("Please describe the image"),
            new ImageContent(new Uri(
                "https://i.pinimg.com/736x/2e/59/a7/2e59a7b18c4ace190ea3756e2029af55--cat-climbing-tree-stock-photos.jpg"),
                "image/jpeg")
        }
    };

    messages.Add(messageWithImage);
    var response = await client.CompleteAsync(messages);
    Console.WriteLine(response);
    return;
}
# endregion

Console.WriteLine("Welcome to the chatbot! Ask me anything.");
while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    var question = Console.ReadLine();

    if (ragDemo && !string.IsNullOrEmpty(question))
    {
        var ragResponse = await ragProvider.SearchEmbeddingsAsync(question);
        if (ragResponse != null)
        {
            var ragMessage = $"To answer the question below, use this supporting information: {ragResponse}";
            messages.Add(new(ChatRole.User, ragMessage));
        }
    }
    
    Console.ForegroundColor = ConsoleColor.Red;

    messages.Add(new(ChatRole.User, question));

    var response = await client.CompleteAsync(messages, toolsDemo ? chatOptions : null);
    
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(response);

    messages.Add(new(ChatRole.Assistant, response.ToString()));

    Console.WriteLine();
}