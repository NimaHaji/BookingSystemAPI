using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add23ValueToUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Role_Valid_Values",
                table: "Users");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Role_Valid_Values",
                table: "Users",
                sql: "[Role] IN (0,1,2,3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Role_Valid_Values",
                table: "Users");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Role_Valid_Values",
                table: "Users",
                sql: "[Role] IN (0, 1)");
        }
    }
}
