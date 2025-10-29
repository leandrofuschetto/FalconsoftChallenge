using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RecruitingChallenge.API.DTOs.Login;
using RecruitingChallenge.DAL;
using RecruitingChallenge.Domain.Models;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
                const string TestDbFile = "test.db";
                if (File.Exists(TestDbFile))
                {
                    try { File.Delete(TestDbFile); } catch { /* ignore if locked */ }
                }

                services.AddDbContext<OrderNowDbContext>(options =>
                {
                    options.UseSqlite($"DataSource={TestDbFile}")
                           .EnableSensitiveDataLogging()
                           .EnableDetailedErrors();
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
                
                db.Database.EnsureCreated();

                if (!db.Users.Any())
                {
                    SeedTestData(db);
                }});
        }

        private OrderNowDbContext _testContext;
        private readonly object _contextLock = new object();

        protected async Task AddToDataBase(params object[] entities)
        {
            GetOrCreateTestContext();

            foreach (var entity in entities)
            {
                await _testContext!.AddAsync(entity);
            }

            await _testContext!.SaveChangesAsync();
            _testContext.ChangeTracker.Clear();
        }

        protected async Task ClearDatabase()
        {
            GetOrCreateTestContext();

            _testContext!.OrderItems.RemoveRange(_testContext.OrderItems);
            _testContext.Orders.RemoveRange(_testContext.Orders);
            _testContext.Products.RemoveRange(_testContext.Products);
            _testContext.Clients.RemoveRange(_testContext.Clients);
            
            await _testContext.SaveChangesAsync();
        }

        protected async Task<TEntity> FindOnDatabase<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            GetOrCreateTestContext();

            var entity = await _testContext
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);

            return entity;
        }

        protected async Task<int> CountOnDatabase<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            GetOrCreateTestContext();

            return await _testContext
                .Set<TEntity>()
                .CountAsync(filter);
        }

        private void GetOrCreateTestContext()
        {
            if (_testContext == null)
            {
                lock (_contextLock)
                {
                    if (_testContext == null)
                    {
                        var scope = Services.CreateScope();
                        _testContext = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _testContext?.Dispose();
            base.Dispose(disposing);
        }

        protected async Task<HttpClient> AuthenticateAsync()
        {
            var loginRequest = new
            {
                UserName = "admin_test",
                Password = "admin1234"
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

        protected HttpContent GetStringContent<T>(T request)
            => new StringContent(
                JsonSerializer.Serialize<T>(request),
                Encoding.UTF8,
                "application/json");

        protected void GetHttpClient() => CreateClient();

        private void SeedTestData(OrderNowDbContext context)
        {
            var user = new RecruitingChallenge.DAL.Entities.UserEntity
            {
                Id = new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"),
                EntryDate = new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                Username = "admin_test",
                Password = "UeZVPwOUUXg1/P/tnFtUy/Xz++GHJDZkf84vcwfQ4jA=",
                Salt = "AdmIN6sEUjja+FymlBj+Lw=="
            };

            context.Users.Add(user);

            context.SaveChanges();
        }
    }
}
