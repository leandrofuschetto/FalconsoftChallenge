using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace RecruitingChallenge.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialTestData : Migration
    {
        private readonly string _environment;

        public SeedInitialTestData()
        {
            _environment = Environment.GetEnvironmentVariable("Environment");
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (_environment == "[PRODUCTION]")
                return;

            //Pass lean1234
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EntryDate", "Username", "Password", "Salt" },
                values: new object[] { new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "leandrof", "o3tAEewYaqQ1TlwTX67FGz+j3n89aEgBzjcLMj+XVcw=", "S1joR6sEUjja+FymlBj+Lw==" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Email", "EntryDate", "LastName", "Name" },
                values: new object[,]
                {
                    { new Guid("161b788d-8706-4cba-9598-f27ba3088238"), "dibumartinez@gmail.com", new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "martinez", "dibu" },
                    { new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), "leomessi@gmail.com", new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "messi", "leo" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "EntryDate", "Name", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("582b1126-be69-4538-acb8-020a3d94c944"), "Description for Product 3", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 3", 1.5m },
                    { new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), "Description for Product 2", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 2", 1.0m },
                    { new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), "Description for Product 1", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product 1", 0.5m }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "ClientId", "EntryDate", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1.5m },
                    { 2, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2.0m },
                    { 3, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1.5m },
                    { 4, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3.0m },
                    { 5, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.5m },
                    { 6, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.0m },
                    { 7, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3.0m },
                    { 8, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m },
                    { 9, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1.0m },
                    { 10, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4.0m },
                    { 11, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.0m },
                    { 12, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m },
                    { 13, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0.5m },
                    { 14, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.0m },
                    { 15, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2.0m },
                    { 16, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3.0m },
                    { 17, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6.0m },
					{ 18, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4.5m },
					{ 19, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m },
                    { 20, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.0m },
                    { 21, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4.0m },
                    { 22, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1.5m },
                    { 23, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6.0m },
					{ 24, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.0m },
                    { 25, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2.0m },
                    { 26, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4.5m },
                    { 27, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3.0m },
                    { 28, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2.0m },
					{ 29, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.5m },
                    { 30, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4.5m },
					{ 31, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3.0m },
                    { 32, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 0.5m },
                    { 33, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m },
                    { 34, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1.5m },
                    { 35, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3.0m },
                    { 36, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2.5m },
                    { 37, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.0m },
					{ 38, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1.5m },
                    { 39, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4.0m },
                    { 40, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.5m },
                    { 41, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0.5m },
					{ 42, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1.0m },
                    { 43, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m },
                    { 44, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6.0m },
                    { 45, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4.5m },
                    { 46, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6.0m },
                    { 47, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4.0m },
                    { 48, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1.0m },
                    { 49, new Guid("161b788d-8706-4cba-9598-f27ba3088238"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1.0m },
                    { 50, new Guid("7530ecf3-1ea6-421e-bbf5-0f12c0705cdb"), new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.0m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("001408dd-2c5d-461b-b037-f47d3818945c"), 27, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 4 },
                    { new Guid("001408dd-2c5d-461b-b037-f47d38189111"), 27, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("09c247f4-e9a7-4a2c-9d45-990ae9c07059"), 31, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 3 },
                    { new Guid("191763b4-d752-4bc1-9d87-4a48ae5f9034"), 18, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("19a6b4f7-0794-4b71-b016-47055a7c5bf7"), 7, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 2 },
                    { new Guid("1c374321-5580-45dc-b37a-00146a697c9a"), 12, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 2 },
                    { new Guid("1c374321-5580-45dc-b37a-00146a697111"), 12, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 1 },
                    { new Guid("1c374321-5580-45dc-b37a-00146a697222"), 12, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("1de2c5a3-1716-40c8-aed5-e70ab343a61b"), 34, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 1 },
                    { new Guid("2092222d-ddee-48fc-82ad-3a26c1cfa211"), 45, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("247a0dc4-aa5e-482d-93b9-408280e29bc8"), 33, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("24a00315-cca2-42da-8332-cea9b47caab7"), 23, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("2576c24b-5666-46c3-ade4-3fd54c1e1140"), 29, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 3 },
                    { new Guid("33718b0b-f58a-43ca-b552-6818300704d3"), 26, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("3a4c9ea3-3fd2-4e3c-ad30-b865c1bf9b1f"), 28, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 2 },
                    { new Guid("41706982-e195-47b1-8949-45750f77e0a9"), 36, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("41706982-e195-47b1-8949-45750f774444"), 36, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 1 },
                    { new Guid("512ee7b9-ff34-4f43-b9ff-df9d5ed94af2"), 40, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 1 },
                    { new Guid("526fb0ed-66b6-4cd9-81ea-51dc64df7559"), 21, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 4 },
                    { new Guid("544a23fd-1184-4ab9-a4ba-ab88cffa1d9b"), 25, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 2 },
                    { new Guid("54650f56-c291-4f6d-91c3-93166cc49f56"), 42, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("54a8d198-2d70-4c86-bc6f-7fbcb7362c5e"), 5, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("54a8d198-2d70-4c86-bc6f-7fbcb7369999"), 5, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 1 },
                    { new Guid("552bb17e-69f4-4c19-a4fd-43fb86681886"), 22, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 1 },
                    { new Guid("581e36a8-af3a-458a-9377-8e87ac040d48"), 41, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 1 },
                    { new Guid("58f738d0-122a-4809-b9b2-214463bcd62f"), 13, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 1 },
                    { new Guid("5d8858eb-9224-4436-a631-c08a83f8104f"), 3, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 3 },
                    { new Guid("655b23f5-5bcd-4fec-96f6-11a6edaf4e28"), 16, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 2 },
                    { new Guid("6598d0a9-921f-4c4c-a201-dee565854fbc"), 6, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("66c208d1-60ff-45db-a09a-6354a3747558"), 4, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 3 },
                    { new Guid("790c82b6-b034-4e73-9c5a-b7d3b94d7d63"), 48, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("820ac57a-e185-4e5b-b17e-9891f8b79896"), 37, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("8702771f-d3a9-42f1-bf58-26d4e43770ef"), 47, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("8702771f-d3a9-42f1-bf58-26d4e4371111"), 47, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 2 },
                    { new Guid("876ede8a-7e57-49c9-b3eb-c6ff3053512a"), 20, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("88bb2772-93b9-4959-a20f-8f777a3d6eff"), 17, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("a4602e32-d97d-4c28-bccf-5d5d053e4743"), 10, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 4 },
                    { new Guid("a4b970d0-23ba-4e54-97f5-817e4a991bd8"), 35, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 3 },
                    { new Guid("a6025948-6657-4534-bd8b-a7adc7f3d95c"), 14, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("a68854b1-047e-49da-a57a-24afe6c776c1"), 19, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("a74e54c4-9a40-49d5-94f7-df8179cf0ad9"), 32, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 1 },
                    { new Guid("bbfdca70-b1dc-41d4-980b-eb757a0b7ffe"), 15, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 2 },
                    { new Guid("c4a200f2-ce7c-4f6e-8ecb-a8cc0d2e6fc6"), 44, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("ce73a183-1c1d-4338-a189-ee8aadffdfe8"), 46, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 4 },
                    { new Guid("cfb75d4f-f639-4200-bf0d-72f9a317b489"), 2, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 4 },
                    { new Guid("cfd33c27-6207-41ea-9f26-32a4914b149f"), 38, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 3 },
                    { new Guid("d44889a4-1029-4c82-9430-07f753545921"), 11, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 4 },
                    { new Guid("d81b1438-49c4-42c2-a409-8fb32f99618a"), 39, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 4 },
                    { new Guid("db29f94c-61ec-43f8-87c5-e52b9c5f8db3"), 8, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("db645290-3f7c-4288-978d-90d0cb48c2ba"), 50, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("db645290-3f7c-4288-978d-90d0cb480000"), 50, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 1 },
                    { new Guid("db645290-3f7c-4288-978d-90d0cb481111"), 50, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("df0b7327-14a2-4ccf-a5e8-569b2aeb2681"), 43, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 },
                    { new Guid("e314ba6c-8a70-4402-9afe-34eb40eaaf92"), 24, new Guid("5b1d7a8c-7c1b-43c3-91c1-3635a4dadcd7"), 1 },
                    { new Guid("e79cf3e7-c28f-49df-bafd-45ac37cf0858"), 9, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("e84eadf3-e0e3-4e43-9d68-33866eb83a01"), 1, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 3 },
                    { new Guid("fc537213-821b-4736-aab6-e691d667b5e0"), 49, new Guid("bdfffdfb-35ab-457b-a32f-bc421dc9c5df"), 2 },
                    { new Guid("fcf7ce1c-4173-4751-a38e-265fc23dde59"), 30, new Guid("582b1126-be69-4538-acb8-020a3d94c944"), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (_environment == "[PRODUCTION]")
                return;

            migrationBuilder.Sql("DELETE FROM [OrderItems]");
            migrationBuilder.Sql("DELETE FROM [Orders]");
            migrationBuilder.Sql("DELETE FROM [Clients]");
            migrationBuilder.Sql("DELETE FROM [Products]");
            migrationBuilder.Sql("DELETE FROM [Users]");
        }
    }
}
