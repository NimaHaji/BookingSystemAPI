using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixingPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Payments",
                newName: "PaymentStatus");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentGatewayStatus",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wage",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentGatewayStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Wage",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Payments",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
