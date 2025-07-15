using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalResearch.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRelMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "wP5ZlKFBMv4i/bFEsJohepRzLOJ97o9on2kHd3zB6pU=", new byte[] { 40, 22, 77, 132, 160, 22, 170, 160, 79, 12, 138, 6, 70, 169, 29, 47, 238, 22, 146, 40, 200, 111, 116, 39, 199, 94, 5, 184, 215, 1, 208, 250 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "v1T3qUmK57bXJSN2D4o2qANUzfEMbaLmCyfBEve/+yQ=", new byte[] { 27, 216, 118, 244, 51, 232, 21, 31, 170, 158, 156, 207, 96, 211, 45, 32, 111, 160, 191, 121, 190, 156, 68, 29, 51, 172, 174, 26, 38, 216, 176, 171 } });
        }
    }
}
