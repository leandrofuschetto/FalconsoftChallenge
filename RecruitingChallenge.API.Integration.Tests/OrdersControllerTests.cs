using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RecruitingChallenge.API.DTOs.Order;
using RecruitingChallenge.Common.Extensions;
using RecruitingChallenge.DAL;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.DAL.Migrations;
using RecruitingChallenge.Domain.Models;
using RecruitingChallenge.Tests.Utilities.Builders.Entities;
using System.Text;
using System.Text.Json;

namespace RecruitingChallenge.API.Integration.Tests
{
    internal class OrdersControllerTests : CustomWebApplicationFactory
    {
        [SetUp]
        public void Setup() 
        {
        }

        [Test]
        public async Task GetOrderById_HappyPath()
        {
            // Arrange
            Guid clientId = Guid.NewGuid();

            var clientEntity = new TestsClientEntityBuilder()
                .WithEmail("email@gmail.com")
                .WithId(clientId)
                .WithFirstName("name")
                .WithLastname("lastname")
                .Build();

            var productEntity = new TestProductEntityBuilder()
                .WithId(Guid.NewGuid())
                .WithUnitPrice(1.05m)
                .WithDescription("Product A")
                .WithEntryDate(new DateTime(2025, 09, 09))
                .WithName("Product A")
                .Build();

            var orderItemEntity = new TestOrderItemEntityBuilder()
                .WithId(Guid.NewGuid())
                .WithQuantity(1)
                .WithProduct(productEntity)
                .Build();

            var orderEntity = new TestOrderEntityBuilder()
                .WithId(1)
                .WithClient(clientEntity)
                .WithEntryDate(new DateTime(2025, 10, 15))
                .WithOrderItems(orderItemEntity)
                .WithTotalAmount(10)
                .Build();

            // Add all related entities at once to avoid dependency issues
            await AddMultipleToDatabase(clientEntity, productEntity, orderItemEntity, orderEntity);

            var httpClient = await AuthenticateAsync();

            // Act
            var response = await httpClient.GetAsync($"api/v1/orders/{orderEntity.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonSerializer.Deserialize<GetOrderResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(orderResponse.Id, Is.EqualTo(orderEntity.Id));
            Assert.That(orderResponse.TotalAmount, Is.EqualTo(orderEntity.TotalAmount));
            Assert.That(orderResponse.ClientEmail, Is.EqualTo(orderEntity.Client.Email));
            
            // Verify order items
            Assert.That(orderResponse.Items, Is.Not.Null);
            Assert.That(orderResponse.Items.Count, Is.EqualTo(1));
            
            var orderItem = orderResponse.Items.First();
            Assert.That(orderItem.Quantity, Is.EqualTo(orderItemEntity.Quantity));
            Assert.That(orderItem.ProductName, Is.EqualTo(orderItemEntity.Product.Name));
            Assert.That(orderItem.UnitPrice, Is.EqualTo(orderItemEntity.Product.UnitPrice));
        }
    }
}
