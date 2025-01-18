using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using CourseAI.Core.Extensions;
using Microsoft.Extensions.Options;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class AIContentGenerator(
    IHttpClientFactory httpClientFactory,
    IOptions<OpenAIOptions> options,
    IOptions<StabilityAIOptions> stabilityOptions,
    IStorageService storageService
) : IContentGenerator
{
    private const string AIRequestUrl = "https://api.openai.com/v1/chat/completions";
    private const string StabilityUrl = "https://api.stability.ai";
    private const string ThumbnailsPath = "thumbnails";

    private readonly string _apiKey = options.Value.ApiKey;
    private readonly string _stabilityKey = stabilityOptions.Value.ApiKey;

    public async Task<string> GenerateContentAsync(string userPrompt, string systemMessage)
    {
        var messages = new[]
        {
            new { role = "system", content = systemMessage },
            new { role = "user", content = userPrompt }
        };

        var requestPayload = new
        {
            model = "gpt-3.5-turbo",
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
        [JsonPropertyName("choices")] public List<Choice> Choices { get; set; }
    }

    private class Choice
    {
        [JsonPropertyName("message")] public Message Message { get; set; }
    }

    private class Message
    {
        [JsonPropertyName("content")] public string Content { get; set; }
    }

    public async Task<ImageGenerationResponse> GenerateImageAsync(string title, bool isRandomPromptWrapper, string? style,
        string? fileName, string? savePath)
    {
        try
        {
            var finalPrompt = title;
            var finalStyle = style;
            if (isRandomPromptWrapper)
            {
                var (randPrompt, randStyle) = thumbnailTemplates[Random.Shared.Next(thumbnailTemplates.Length)];
                finalPrompt = string.Format(randPrompt, title);
                finalStyle = randStyle;
            }

            var requestPayload = new
            {
                text_prompts = new[]
                {
                    new { text = finalPrompt }
                },
                cfg_scale = 7,
                height = 512,
                width = 512,
                samples = 1,
                steps = 30,
                style_preset = finalStyle,
            };

            using var httpClient = httpClientFactory.CreateClient();
            var requestJson = JsonSerializer.Serialize(requestPayload);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var engineId = "stable-diffusion-v1-6";
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{StabilityUrl}/v1/generation/{engineId}/text-to-image"
            );
            request.Headers.Add("Authorization", $"Bearer {_stabilityKey}");
            request.Content = requestContent;

            using var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ImageGenerationResponse
                {
                    Success = false,
                    Error = $"Stability AI API request failed with status {response.StatusCode}: {responseContent}"
                };
            }

            var result = JsonSerializer.Deserialize<StabilityResponse>(responseContent);

            if (result?.Artifacts?.FirstOrDefault()?.Base64 == null)
            {
                return new ImageGenerationResponse
                {
                    Success = false,
                    Error = "No image generated"
                };
            }

            var originalImageBytes = Convert.FromBase64String(result.Artifacts[0].Base64);
            var optimizedImageBytes = ImageOptimizer.OptimizeImage(originalImageBytes);
            var optimizedBase64 = Convert.ToBase64String(optimizedImageBytes);

            var finalFileName = fileName ?? StringExtensions.ToUrl(title);

            var imageUrl = await storageService.SaveImageAsync(
                optimizedBase64,
                finalFileName,
                savePath ?? ThumbnailsPath
            );

            return new ImageGenerationResponse
            {
                Success = true,
                FileName = finalFileName,
                ImageUrl = imageUrl
            };
        }
        catch (Exception ex)
        {
            return new ImageGenerationResponse
            {
                Success = false,
                Error = $"Failed to generate image: {ex.Message}"
            };
        }
    }

    public class StabilityResponse
    {
        [JsonPropertyName("artifacts")] public List<Artifact> Artifacts { get; set; } = new();
    }

    public class Artifact
    {
        [JsonPropertyName("base64")] public string Base64 { get; set; } = string.Empty;

        [JsonPropertyName("finish_reason")] public string FinishReason { get; set; } = string.Empty;

        [JsonPropertyName("seed")] public long Seed { get; set; }
    }

    private readonly (string prompt, string style)[] thumbnailTemplates = new[]
    {
        ("Modern educational workspace visualizing {0}, minimalist tech design with floating geometric shapes, soft blue gradient background, professional lighting",
            "digital-art"),
        ("Professional abstract representation of {0}, isometric design with subtle grid patterns, modern corporate colors, clean lines and shapes, smooth gradients",
            "isometric"),
        ("Futuristic learning environment for {0}, modern minimal design with glowing elements, professional dark theme with accent lighting, subtle tech elements",
            "photographic"),
        ("Contemporary educational setting for {0}, clean workspace composition, soft ambient lighting, professional color palette",
            "photographic"),
        ("Professional blueprint-style visualization of {0}, modern tech elements with geometric patterns, subtle grid overlay, cool gradient background",
            "3d-model"),
        ("Elegant modern interpretation of {0}, flowing gradient composition, professional minimalist design, subtle depth elements",
            "cinematic"),
        ("Dynamic professional visualization of {0}, modern tech-inspired elements, clean geometric shapes, subtle motion effects, corporate color scheme",
            "digital-art")
    };
}