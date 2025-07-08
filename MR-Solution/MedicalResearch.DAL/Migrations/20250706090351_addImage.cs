using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalResearch.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Users",
                type: "bytea",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Image", "Password", "PasswordSalt" },
                values: new object[] { null, "v1T3qUmK57bXJSN2D4o2qANUzfEMbaLmCyfBEve/+yQ=", new byte[] { 27, 216, 118, 244, 51, 232, 21, 31, 170, 158, 156, 207, 96, 211, 45, 32, 111, 160, 191, 121, 190, 156, 68, 29, 51, 172, 174, 26, 38, 216, 176, 171 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "l8jWYAI+LjizTpMYFGqAJ5MYLIQV0AreG/wCjFt7I6k=", new byte[] { 82, 56, 70, 177, 92, 195, 222, 64, 171, 216, 248, 52, 87, 208, 81, 194, 51, 40, 94, 129, 222, 0, 213, 101, 225, 218, 18, 148, 202, 222, 118, 42 } });
        }
    }
}
