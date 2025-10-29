using Azure;
using Azure.Identity;
using FluentAssertions;
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
        public async Task GetOrderById_HappyPath()
        {
            // Arrange
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase(id: 100);

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

            orderResponse.Id.Should().Be(orderEntity.Id);
            orderResponse.TotalAmount.Should().Be(orderEntity.TotalAmount);
            orderResponse.ClientEmail.Should().Be(orderEntity.Client.Email);

            orderResponse.Items.Should().NotBeNull();
            orderResponse.Items.Should().HaveCount(1);

            var orderItem = orderResponse.Items.First();
            orderItem.Quantity.Should().Be(orderItemEntity.Quantity);
            orderItem.ProductName.Should().Be(orderItemEntity.Product.Name);
            orderItem.UnitPrice.Should().Be(orderItemEntity.Product.UnitPrice);
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("ORDER_NOT_FOUND");
        }

        [Test]
        public async Task UpdateOrderStatus_HappyPath()
        {
            // Arrange
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase(status: EOrderStatus.Pending);

            var httpClient = await AuthenticateAsync();

            var statusExpected = EOrderStatus.Processing;

            var updateOrderRequest = new UpdateOrderRequest() { NewStatus = statusExpected };

            // Act
            var response = await httpClient.PatchAsync($"api/v1/orders/{orderEntity.Id}", GetStringContent(updateOrderRequest));

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var order = await FindOnDatabase<OrderEntity>(o => o.Id == orderEntity.Id);
            order.Status.Should().Be(statusExpected);
        }

        [TestCase(3.0, 1.5, 2, 3)]
        [TestCase(3.0, 1.0, 4, 1)]
        public async Task UpdateOrderItemQuantity_HappyPath(decimal totalAmount, decimal unitPrice, int actualQuantity, int newQuantity)
        {
            // Arrange
            decimal totalAmountExpected = newQuantity * unitPrice;
            
            (OrderItemEntity orderItemEntity, OrderEntity orderEntity) = await AddBasicDataEntitiesToDataBase(
                id: 100,
                status: EOrderStatus.Pending,
                totalAmount: totalAmount,
                quantity: actualQuantity,
                unitPrice: unitPrice);

            var httpClient = await AuthenticateAsync();

            var updateOrderItemRequest = new UpdateOrderItemQuantityRequest() { Quantity = newQuantity };

            // Act
            var response = await httpClient.PatchAsync($"api/v1/orders/{orderEntity.Id}/orderItems/{orderItemEntity.Id}", GetStringContent(updateOrderItemRequest));

            // Assert
            response.EnsureSuccessStatusCode();

            var order = await FindOnDatabase<OrderEntity>(o => o.Id == orderEntity.Id);
            var orderItem = await FindOnDatabase<OrderItemEntity>(oi => oi.Id == orderItemEntity.Id);

            order.TotalAmount.Should().Be(totalAmountExpected);
            orderItem.Quantity.Should().Be(newQuantity);
        }

        private async Task<(OrderItemEntity orderItemEntity, OrderEntity orderEntity)> AddBasicDataEntitiesToDataBase(
            int id = 1, 
            EOrderStatus status = EOrderStatus.Processing, 
            int quantity = 1,
            decimal totalAmount = 10m,
            decimal unitPrice = 1.05m)
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
                .WithUnitPrice(unitPrice)
                .WithDescription("Product A")
                .WithEntryDate(new DateTime(2025, 09, 09))
                .WithName("Product A")
                .Build();

            var orderItemEntity = new TestOrderItemEntityBuilder()
                .WithId(Guid.NewGuid())
                .WithQuantity(quantity)
                .WithProduct(productEntity)
                .Build();

            var orderEntity = new TestOrderEntityBuilder()
                .WithId(id)
                .WithClient(clientEntity)
                .WithEntryDate(new DateTime(2025, 10, 15))
                .WithOrderItems(orderItemEntity)
                .WithStatus(status)
                .WithTotalAmount(totalAmount)
                .Build();

            await AddToDataBase(clientEntity, productEntity, orderItemEntity, orderEntity);
            return (orderItemEntity, orderEntity);
        }

        private HttpContent GetStringContent<T>(T request)
            => new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json");
    }
}
