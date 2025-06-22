## GenerativeBot
This is a project to answer and send messages using a generative AI model to Telegram. **Still under development!!!**

### Features
- Historical data storage with RavenDB using the Vector search.
- .NET Aspire to configure and manage all containers.
- Using the Olamma with the gemma 3 model.

## Requirements
To run this project you will need the following prerequisites:

- .NET SDK Version 9.0 or higher.
- Docker for desktop (recommended).
- A good GPU to use the model. The rig that was used to develop this project has an NVIDIA GeForce RTX 4070 super. If you have lower specs I recommend to change the parameters of the model to a lower value.

## Environment Setup
1. The Docker for desktop is recommended because the Docker for desktop app configure all the require network you need to run your containers, but of course, you can try configure all manually and only use the WSL2.
4. You can change the model running on the appsettings.json on the AppHost project. The default model is the Deepseek with 14 billions parameter, but you can change to the other models that are available on the Ollama repository.
```json
"LanguageModel": {
    "Model": "gemma3:12b",
    "Port": 11434
}
```