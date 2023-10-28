using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_API.Migrations
{
    /// <inheritdoc />
    public partial class varchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "varchar(35)",
                maxLength: 14,
                nullable: false,
                defaultValue: "ApplicationUser",
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldDefaultValue: "ApplicationUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "ApplicationUser",
                oldClrType: typeof(string),
                oldType: "varchar(35)",
                oldMaxLength: 14,
                oldDefaultValue: "ApplicationUser");
        }
    }
}
