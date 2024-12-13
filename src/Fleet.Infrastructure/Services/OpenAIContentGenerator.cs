using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fleet.Application.Options;
using Fleet.Application.Services;
using Microsoft.Extensions.Options;
using NeerCore.DependencyInjection;

namespace Fleet.Infrastructure.Services;

[Service]
public class OpenAIContentGenerator(IHttpClientFactory httpClientFactory, IOptions<OpenAIOptions> options)
    : IContentGenerator
{
    private const string? AIRequestUrl = "https://api.openai.com/v1/chat/completions";
    private readonly string _apiKey = options.Value.ApiKey;

    public async Task<string> GenerateContentAsync(string userPrompt, string systemMessage)
    {
        var messages = new[]
        {
            new { role = "system", content = systemMessage },
            new { role = "user", content = userPrompt }
        };

        var requestPayload = new
        {
            model = "gpt-3.5-turbo", // or "o1-mini" if you have access
            messages,
            max_tokens = 2500,
            temperature = 0.7
        };

        using var httpClient = httpClientFactory.CreateClient();
        var requestJson = JsonSerializer.Serialize(requestPayload);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, AIRequestUrl);
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = requestContent;

        using var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"OpenAI API request failed with status {response.StatusCode}: {error}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var openAiResponse = JsonSerializer.Deserialize<OpenAiResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return openAiResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim() ?? string.Empty;
    }

    private class OpenAiResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    private class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    private class Message
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
