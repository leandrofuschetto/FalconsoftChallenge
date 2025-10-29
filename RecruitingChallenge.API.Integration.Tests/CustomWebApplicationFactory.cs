using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RecruitingChallenge.API.DTOs.Login;
using RecruitingChallenge.DAL;
using RecruitingChallenge.Domain.Models;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace RecruitingChallenge.API.Integration.Tests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<OrderNowDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestsDb")
                           .EnableSensitiveDataLogging()
                           .EnableDetailedErrors();
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
                db.Database.EnsureCreated();

                SeedTestData(db);
            });
        }

        protected async Task AddToDateBase<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();

            await context.Set<TEntity>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        protected async Task<HttpClient> AuthenticateAsync()
        {
            var loginRequest = new
            {
                UserName = "leandrof",
                Password = "lean1234"
            };

            var loginJson = JsonSerializer.Serialize(loginRequest);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var httpClient = CreateClient();

            var loginResponse = await httpClient.PostAsync("api/v1/authentication", loginContent);
            loginResponse.EnsureSuccessStatusCode();

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

            return httpClient;
        }

        private void SeedTestData(OrderNowDbContext context)
        {
            var user = new RecruitingChallenge.DAL.Entities.UserEntity
            {
                Id = new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"),
                EntryDate = new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                Username = "leandrof",
                Password = "o3tAEewYaqQ1TlwTX67FGz+j3n89aEgBzjcLMj+XVcw=",
                Salt = "S1joR6sEUjja+FymlBj+Lw=="
            };

            context.Users.Add(user);

            context.SaveChanges();
        }
    }
}
