using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RecruitingChallenge.API.DTOs.Login;
using RecruitingChallenge.API.DTOs.Order;
using System.Text.Json;

namespace RecruitingChallenge.API.Integration.Tests.Controllers
{
    internal class AuthenticationControllerTests : CustomWebApplicationFactory
    {
        private HttpClient _httpClient;

        [SetUp]
        public async Task Setup()
        {
            _httpClient = CreateClient();

            await ClearDatabase();
        }

        [Test]
        public async Task Authentication_HappyPath()
        {
            // Arrange
            var loginRequest = new LoginRequest();
            loginRequest.UserName = "admin_test";
            loginRequest.Password = "admin1234";

            // Act
            var response = await _httpClient.PostAsync("api/v1/authentication", GetStringContent(loginRequest));

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            loginResponse.Token.Should().NotBeNull();
        }

        [TestCase("admin_test", "passwrong")]
        [TestCase("user_inexisten", "admin1234")]
        public async Task Authentication_BadCredentials_ShouldThrownAuthenticationError(string username, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest();
            loginRequest.UserName = username;
            loginRequest.Password = password;

            // Act
            var response = await _httpClient.PostAsync("api/v1/authentication", GetStringContent(loginRequest));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("AUTHENTICATION_ERROR_EXCEPTION");
        }
    }
}
