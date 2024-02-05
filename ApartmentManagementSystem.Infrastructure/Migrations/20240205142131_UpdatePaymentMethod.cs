using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Payment",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "Payment",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Payment",
                newName: "PaymentDate");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
