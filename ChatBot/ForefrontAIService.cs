using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatBot
{
    public class ForefrontAIService
    {
        private const string API_URL = "https://api.forefront.ai/v1/chat/completions";
        private const string API_KEY = "sk-TWnumsSzfMKr6SUivHBzgXTnBeZQ5sKm";

        private readonly HttpClient _httpClient;

        public ForefrontAIService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
        }

        public async Task<(string aiResponse, Usage usage)> GetAIResponseAsync(string userMessage)
        {
            try
            {
                var requestBody = new
                {
                    model = "mistralai/Mistral-7B-v0.1",
                    messages = new[]
                    {
                    new { role = "user", content = userMessage }
                },
                    max_tokens = 200,
                    temperature = 0.5
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(API_URL, content);
                var result = await response.Content.ReadAsStringAsync();

                var responseObject = JsonSerializer.Deserialize<ForefrontResponse>(result,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                string aiResponse = responseObject?.Choices?[0]?.Message?.Content ?? "No response received.";
                Usage usageData = responseObject?.Usage ?? new Usage();

                return (aiResponse, usageData);
            }
            catch (Exception ex)
            {
                return ($"Error: {ex.Message}", new Usage());
            }
        }
    }
}
