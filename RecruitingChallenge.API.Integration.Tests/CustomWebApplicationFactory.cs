using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecruitingChallenge.DAL;
using System.Linq;

namespace RecruitingChallenge.API.Integration.Tests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureServices(services =>
            {
                // Add InMemory database for testing
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
                
                // Seed test data
                SeedTestData(db);
            });
        }

        private void SeedTestData(OrderNowDbContext context)
        {
            // Add test user
            var user = new RecruitingChallenge.DAL.Entities.UserEntity
            {
                Id = new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"),
                EntryDate = new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                Username = "leandrof",
                Password = "o3tAEewYaqQ1TlwTX67FGz+j3n89aEgBzjcLMj+XVcw=",
                Salt = "S1joR6sEUjja+FymlBj+Lw=="
            };
            context.Users.Add(user);

            // Add test client
            var client = new RecruitingChallenge.DAL.Entities.ClientEntity
            {
                Id = new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"),
                Email = "leomessi@gmail.com",
                EntryDate = new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                LastName = "messi",
                Name = "leo"
            };
            context.Clients.Add(client);

            // Add test products
            var products = new[]
            {
                new RecruitingChallenge.DAL.Entities.ProductEntity
                {
                    Id = new Guid("582b1126-be69-4538-acb8-020a3d94c944"),
                    Description = "Description for Product 3",
                    EntryDate = new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Name = "Product 3",
                    UnitPrice = 1.5m
                },
                new RecruitingChallenge.DAL.Entities.ProductEntity
                {
                    Id = new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"),
                    Description = "Description for Product 2",
                    EntryDate = new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Name = "Product 2",
                    UnitPrice = 1.0m
                },
                new RecruitingChallenge.DAL.Entities.ProductEntity
                {
                    Id = new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"),
                    Description = "Description for Product 1",
                    EntryDate = new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Name = "Product 1",
                    UnitPrice = 0.5m
                }
            };
            context.Products.AddRange(products);

            // Add test order
            var order = new RecruitingChallenge.DAL.Entities.OrderEntity
            {
                Id = 1,
                ClientId = new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"),
                EntryDate = new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                Status = RecruitingChallenge.Domain.Enums.EOrderStatus.Delivered,
                TotalAmount = 1.5m
            };
            context.Orders.Add(order);

            // Add test order item
            var orderItem = new RecruitingChallenge.DAL.Entities.OrderItemEntity
            {
                Id = new Guid("e84eadf3-e0e3-4e43-9d68-33866eb83a01"),
                OrderId = 1,
                ProductId = new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"),
                Quantity = 3
            };
            context.OrderItems.Add(orderItem);

            context.SaveChanges();
        }

        public void PrintDatabaseContents()
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
            
            Console.WriteLine("=== DATABASE CONTENTS ===");
            
            // Users
            var users = context.Users.ToList();
            Console.WriteLine($"\n--- USERS ({users.Count}) ---");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Password: {user.Password}, Salt: {user.Salt}");
            }
            
            // Clients
            var clients = context.Clients.ToList();
            Console.WriteLine($"\n--- CLIENTS ({clients.Count}) ---");
            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id}, Name: {client.Name} {client.LastName}, Email: {client.Email}");
            }
            
            // Products
            var products = context.Products.ToList();
            Console.WriteLine($"\n--- PRODUCTS ({products.Count}) ---");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.UnitPrice}, Description: {product.Description}");
            }
            
            // Orders
            var orders = context.Orders.ToList();
            Console.WriteLine($"\n--- ORDERS ({orders.Count}) ---");
            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.Id}, ClientId: {order.ClientId}, Status: {order.Status}, Total: {order.TotalAmount}, Date: {order.EntryDate}");
            }
            
            // Order Items
            var orderItems = context.OrderItems.ToList();
            Console.WriteLine($"\n--- ORDER ITEMS ({orderItems.Count}) ---");
            foreach (var item in orderItems)
            {
                Console.WriteLine($"ID: {item.Id}, OrderId: {item.OrderId}, ProductId: {item.ProductId}, Quantity: {item.Quantity}");
            }
            
            Console.WriteLine("=== END DATABASE CONTENTS ===\n");
        }
    }
}
