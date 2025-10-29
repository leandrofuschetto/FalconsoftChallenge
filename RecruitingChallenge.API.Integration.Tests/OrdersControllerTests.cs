using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RecruitingChallenge.API.DTOs.Order;
using RecruitingChallenge.Common.Extensions;
using RecruitingChallenge.DAL;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.DAL.Migrations;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Models;
using RecruitingChallenge.Tests.Utilities.Builders.Entities;
using System.Text;
using System.Text.Json;

namespace RecruitingChallenge.API.Integration.Tests
{
    internal class OrdersControllerTests : CustomWebApplicationFactory
    {
        [SetUp]
        public async Task Setup() 
        {
            await ClearDatabase();
        }

        [Test]
        public async Task GetOrderById_HappyPath(int orderId)
        {
            // Arrange
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase();

            var httpClient = await AuthenticateAsync();

            // Act
            var response = await httpClient.GetAsync($"api/v1/orders/{orderEntity.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var orderResponse = System.Text.Json.JsonSerializer.Deserialize<GetOrderResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(orderResponse.Id, Is.EqualTo(orderEntity.Id));
            Assert.That(orderResponse.TotalAmount, Is.EqualTo(orderEntity.TotalAmount));
            Assert.That(orderResponse.ClientEmail, Is.EqualTo(orderEntity.Client.Email));

            Assert.That(orderResponse.Items, Is.Not.Null);
            Assert.That(orderResponse.Items.Count, Is.EqualTo(1));

            var orderItem = orderResponse.Items.First();
            Assert.That(orderItem.Quantity, Is.EqualTo(orderItemEntity.Quantity));
            Assert.That(orderItem.ProductName, Is.EqualTo(orderItemEntity.Product.Name));
            Assert.That(orderItem.UnitPrice, Is.EqualTo(orderItemEntity.Product.UnitPrice));
        }

        [Test]
        public async Task GetOrderById_OrderNotExistent_ShouldThrowOrderNotFound()
        {
            // Arrange
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase();

            var httpClient = await AuthenticateAsync();

            // Act
            var response = await httpClient.GetAsync($"api/v1/orders/{10}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(content.Contains("ORDER_NOT_FOUND"));
        }

        [Test]
        public async Task UpdateOrderStatus_HappyPath()
        {
            // Arrange
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase(EOrderStatus.Pending);

            var httpClient = await AuthenticateAsync();

            var updateOrderRequest = new UpdateOrderRequest() { NewStatus = EOrderStatus.Processing };

            // Act
            var response = await httpClient.PatchAsync($"api/v1/orders/{10}", GetStringContent(updateOrderRequest));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));
        }

        private async Task<(OrderItemEntity orderItemEntity, OrderEntity orderEntity)> AddBasicDataEntitiesToDataBase(EOrderStatus status = EOrderStatus.Processing)
        {
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
                .WithStatus(status)
                .WithTotalAmount(10)
                .Build();

            await AddToDataBase(clientEntity, productEntity, orderItemEntity, orderEntity);
            return (orderItemEntity, orderEntity);
        }

        private HttpContent GetStringContent(UpdateOrderRequest updateOrderRequest)
            => new StringContent(
                JsonConvert.SerializeObject(updateOrderRequest),
                Encoding.UTF8,
                "application/json");
    }
}
