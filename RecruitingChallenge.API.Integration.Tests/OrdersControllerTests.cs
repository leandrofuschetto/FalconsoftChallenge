using NUnit.Framework;
using System.Text;
using System.Text.Json;

namespace RecruitingChallenge.API.Integration.Tests
{
    internal class OrdersControllerTests : CustomWebApplicationFactory
    {
        private HttpClient _client;

        [SetUp]
        public void Setup() 
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task GetOrderById()
        {
            var loginRequest = new
            {
                UserName = "leandrof",
                Password = "lean1234"
            };

            var loginJson = JsonSerializer.Serialize(loginRequest);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var loginResponse = await _client.PostAsync("api/v1/authentication", loginContent);
            loginResponse.EnsureSuccessStatusCode();

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

            // Act
            var response = await _client.GetAsync($"api/v1/orders/{1}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
