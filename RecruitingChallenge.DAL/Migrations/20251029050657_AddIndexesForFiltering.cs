using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitingChallenge.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesForFiltering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_EntryDate",
                table: "Orders",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TotalAmount",
                table: "Orders",
                column: "TotalAmount");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_EntryDate",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Status",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TotalAmount",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Email",
                table: "Clients");
        }
    }
}
