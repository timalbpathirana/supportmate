# SupportMate.Api

This project is a backend API for an IT support agent powered by OpenAI. It's designed to answer user questions based on verified company policies.

## Features

- **OpenAI Integration**: Utilizes OpenAI's GPT-3.5-turbo model to generate responses.
- **API Endpoints**: Exposes an endpoint to ask questions and receive answers.
- **System Prompt**: Configured to behave as a helpful IT support agent.

## Getting Started

### Prerequisites

- .NET 6.0 SDK
- An OpenAI API key

### Installation

1.  Clone the repository:
    ```sh
    git clone <repository-url>
    ```
2.  Navigate to the project directory:
    ```sh
    cd SupportMate.Api
    ```
3.  Configure your OpenAI API key in `appsettings.Development.json`:
    ```json
    {
      "OpenAI": {
        "ApiKey": "YOUR_OPENAI_API_KEY"
      }
    }
    ```
4.  Run the application:
    ```sh
    dotnet run
    ```

## API Usage

### /ask

This endpoint receives a user's question and returns an answer from the AI.

- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "question": "Your question here"
  }
  ```
- **Response**:
  ```json
  {
    "answer": "The AI's answer here"
  }
  ```

## Project Structure

- **Controllers**: Contains the API controllers.
  - `AskController`: Handles the `/ask` endpoint.
- **Services**: Contains the business logic.
  - `OpenAIService`: Interacts with the OpenAI API.
- **Models**: Contains the data models.
  - `OpenAIApi.cs`: DTOs for OpenAI API requests and responses.
- **Prompts**: Contains the system prompt for the AI.
  - `SystemPrompt.json`: Defines the AI's persona.
- **Program.cs**: The application's entry point.
- **SupportMate.Api.csproj**: The project file, containing dependencies.

## Next Steps

1.  **Implement the `/uploadDocs` endpoint**: The `UploadDocsController` is currently a placeholder. This endpoint could be used to upload and process documents containing company policies, which the AI could then use to answer questions.
2.  **Add Authentication and Authorization**: Secure the API endpoints to ensure that only authorized users can access them.
3.  **Improve Error Handling**: Enhance the error handling to provide more specific and useful error messages.
4.  **Implement Logging and Monitoring**: Add a robust logging and monitoring solution to track the application's performance and identify issues.
5.  **Write Unit and Integration Tests**: Create a suite of tests to ensure the application's correctness and prevent regressions.
