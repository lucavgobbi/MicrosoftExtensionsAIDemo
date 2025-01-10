# Microsoft.Extensions.AI Demo
This repo contains a demo of how to use the Microsoft.Extensions.AI library.

# Disclaimer
The code is not intended for production and has been written with the purpose of clearly presenting the library concepts. No best practices nor patterns were used when writing this code. 

# Requirements
## Ollama
All local models used in the application are served by Ollama.
You must install Ollama in order to execute those models. 
Install instructions and requirements can be found on https://ollama.com.

The following models where downloaded from Ollama
- llama3.1:8b
- llama3.2
- qwen2.5:7b

You can change the models used or add extra models by modifying `ChatClientFactory.cs`

## Azure Open AI
Azure Open AI can be used to run this application.
You can follow this [link](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/create-resource?pivots=web-portal) to deploy the models on Azure.

The code expects gpt-4o to be deployed on your endpoint as well `OPENAI_ENDPOINT` and `OPENAI_KEY` to be set in your environment variables, alternatively you can modify `ChatClientFactory.cs` to use the values directly.

## Dotnet 8 SDK
You can download the SDK from [here](https://dotnet.microsoft.com/download/dotnet/8.0)

# How to run
1. Make sure to have the requirements installed 
2. Start Ollama by running `ollama serve`
3. Ensure the required models are downloaded
4. Clone the repository
5. Navigate to directory
6. Run `dotnet run`