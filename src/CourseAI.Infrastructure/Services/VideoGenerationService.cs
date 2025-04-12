using System.Text;
using System.Text.Json;
using CourseAI.Application.Services;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

public class VideoGenerationResponse
{
    public bool Success { get; set; }
    public string? VideoUrl { get; set; }
    public string? FileName { get; set; }
    public string? Error { get; set; }
    public string? Id { get; set; }
}

public class DIDPresenter
{
    public string Source_Url { get; set; } = "d-id://talents/anna"; // Default presenter
    public string Driver_Url { get; set; } = "bank://wav/driver"; // Audio driver
}

public class DIDResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public string Status { get; set; }
    public IDictionary<string, string> Result { get; set; }
}

[Service]
public class VideoGenerationService(
    IHttpClientFactory httpClientFactory,
    IStorageService storageService,
    IConfiguration configuration)
{
    private readonly string _didApiKey = configuration["DID:ApiKey"] 
                                         ?? throw new ArgumentNullException("DID API key not configured");

    private const string _didApiUrl = "https://api.d-id.com";
    private const string VideosPath = "videos";

    public async Task<VideoGenerationResponse> GenerateVideoAsync(
        string content, 
        string? fileName = null,
        string? savePath = null)
    {
        try
        {
            // 1. Create talk request
            var requestPayload = new
            {
                script = new
                {
                    type = "text",
                    input = content,
                    provider = new
                    {
                        type = "microsoft",
                        voice_id = "en-US-JennyNeural"
                    }
                },
                presenter = new DIDPresenter(),
                config = new
                {
                    stitch = true
                }
            };

            using var httpClient = httpClientFactory.CreateClient();
            var requestJson = JsonSerializer.Serialize(requestPayload);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_didApiUrl}/talks"
            );
            request.Headers.Add("Authorization", $"Bearer {_didApiKey}");
            request.Content = requestContent;

            using var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new VideoGenerationResponse
                {
                    Success = false,
                    Error = $"D-ID API request failed with status {response.StatusCode}: {responseContent}"
                };
            }

            var result = JsonSerializer.Deserialize<DIDResponse>(responseContent);

            if (result?.Id == null)
            {
                return new VideoGenerationResponse
                {
                    Success = false,
                    Error = "No video ID generated"
                };
            }

            // 2. Poll for completion
            var videoUrl = await WaitForVideoCompletionAsync(result.Id);
            
            if (string.IsNullOrEmpty(videoUrl))
            {
                return new VideoGenerationResponse
                {
                    Success = false,
                    Error = "Video generation timed out or failed"
                };
            }

            // 3. Download and store video
            using var videoResponse = await httpClient.GetAsync(videoUrl);
            var videoBytes = await videoResponse.Content.ReadAsByteArrayAsync();
            
            var finalFileName = fileName ?? $"video_{result.Id}";
            var storedVideoUrl = await storageService.SaveVideoAsync(
                videoBytes,
                finalFileName,
                savePath ?? VideosPath
            );

            return new VideoGenerationResponse
            {
                Success = true,
                FileName = finalFileName,
                VideoUrl = storedVideoUrl,
                Id = result.Id
            };
        }
        catch (Exception ex)
        {
            return new VideoGenerationResponse
            {
                Success = false,
                Error = $"Failed to generate video: {ex.Message}"
            };
        }
    }

    private async Task<string?> WaitForVideoCompletionAsync(string id, int maxAttempts = 30)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var attempt = 0;

        while (attempt < maxAttempts)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_didApiUrl}/talks/{id}"
            );
            request.Headers.Add("Authorization", $"Bearer {_didApiKey}");

            using var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DIDResponse>(responseContent);
                
                if (result?.Status == "done" && result.Result?.ContainsKey("video_url") == true)
                {
                    return result.Result["video_url"];
                }
                else if (result?.Status == "failed")
                {
                    return null;
                }
            }

            attempt++;
            await Task.Delay(2000); // Wait 2 seconds between checks
        }

        return null;
    }
}