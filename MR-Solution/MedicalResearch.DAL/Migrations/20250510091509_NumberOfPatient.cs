using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalResearch.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NumberOfPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PaswordSalt" },
                values: new object[] { "NN2UYK0GaglNHHPmmtV+PWZwidOXz2FGVOyijjcJPkQ=", new byte[] { 108, 84, 39, 88, 8, 173, 154, 70, 104, 165, 114, 83, 94, 31, 23, 149, 2, 55, 233, 24, 111, 98, 182, 231, 121, 154, 211, 207, 114, 42, 192, 1 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Patients");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PaswordSalt" },
                values: new object[] { "3bu3siTYGhzJpkFjDqOKhsJrXPcczhf2uyVKSkSuH/0=", new byte[] { 40, 200, 213, 204, 217, 250, 135, 110, 27, 232, 124, 154, 228, 27, 82, 221, 124, 68, 216, 157, 79, 122, 104, 179, 78, 185, 172, 117, 214, 140, 132, 172 } });
        }
    }
}
