using System.Net.Http.Headers;
using System.Text;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using Microsoft.Extensions.Options;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services
{
    [Service]
    public class MailgunService(IHttpClientFactory httpClientFactory, IOptions<MailgunOptions> options) : IMailgunService
    {
        private const string MailgunApiUrlTemplate = "https://api.mailgun.net/v3/{0}/messages";

        private readonly string _apiKey = options.Value.ApiKey;
        private readonly string _mailgunDomain = options.Value.Domain;

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var client = httpClientFactory.CreateClient();

            // Prepare form data as required by Mailgun
            var formContent = new MultipartFormDataContent
            {
                { new StringContent($"no-reply@{_mailgunDomain}"), "from" },
                { new StringContent(to), "to" },
                { new StringContent(subject), "subject" },
                { new StringContent(htmlContent), "html" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(MailgunApiUrlTemplate, _mailgunDomain))
            {
                Content = formContent,
            };

            // Add Basic Authentication header with API key
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_apiKey}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Mailgun API request failed with status {response.StatusCode}: {error}");
            }
        }
    }
}
