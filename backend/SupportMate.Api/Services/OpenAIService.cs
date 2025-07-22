using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SupportMate.Api.Models;

namespace SupportMate.Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _systemPrompt;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(IConfiguration config, IHttpClientFactory httpClientFactory, ILogger<OpenAIService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("OpenAIClient");
            _apiKey = config["OpenAI:ApiKey"];
            _systemPrompt = config["SystemPrompt"];
            _logger = logger;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> AskOpenAIAsync(string userQuestion)
        {
            var payload = new OpenAIRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new[]
                {
                    new Message { Role = "system", Content = _systemPrompt },
                    new Message { Role = "user", Content = userQuestion }
                },
                MaxTokens = 300,
                Temperature = 0.3
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(json);

                if (openAIResponse?.Choices != null && openAIResponse.Choices.Length > 0 && openAIResponse.Choices[0].Message != null && !string.IsNullOrEmpty(openAIResponse.Choices[0].Message.Content))
                {
                    return openAIResponse.Choices[0].Message.Content;
                }

                _logger.LogWarning("Received an empty or invalid response from OpenAI API.");
                return "I'm sorry, I couldn't get a response. Please try again.";
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "An error occurred when calling the OpenAI API.");
                return "There was an error communicating with the AI service. Please try again later.";
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "An error occurred when processing the response from OpenAI API.");
                return "There was an error processing the AI's response. Please try again later.";
            }
        }
    }
}
