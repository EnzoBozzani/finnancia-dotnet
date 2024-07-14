using System.Text;
using Newtonsoft.Json;
using DotNetEnv;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.DTOs.GenAI;

namespace FinnanciaCSharp.Services
{
    public class GenAIService : IGenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GenAIService()
        {
            Env.Load();
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")!;
            _httpClient = new HttpClient();
        }

        public async Task<string?> ChatWithAIAsync()
        {
            var model = "gemini-1.5-flash";

            var requestContent = new
            {
                contents = new[]
                    {
                        new
                        {
                            role = "user",
                            parts = new[]
                            {
                                new { text = "O que é o Santos FC?" }
                            }
                        },
                    }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}")
            {
                Content = jsonContent
            };

            HttpResponseMessage res = await _httpClient.SendAsync(requestMessage);

            res.EnsureSuccessStatusCode();

            var responseContent = await res.Content.ReadAsStringAsync();
            var chatResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

            if (chatResponse == null)
            {
                return null;
            }

            return chatResponse.candidates[0].content.parts[0].text;
        }
    }
}
