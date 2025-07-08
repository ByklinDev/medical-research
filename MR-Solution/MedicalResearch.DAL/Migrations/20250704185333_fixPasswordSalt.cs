using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalResearch.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixPasswordSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaswordSalt",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "l8jWYAI+LjizTpMYFGqAJ5MYLIQV0AreG/wCjFt7I6k=", new byte[] { 82, 56, 70, 177, 92, 195, 222, 64, 171, 216, 248, 52, 87, 208, 81, 194, 51, 40, 94, 129, 222, 0, 213, 101, 225, 218, 18, 148, 202, 222, 118, 42 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "PaswordSalt");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PaswordSalt" },
                values: new object[] { "EtnPrreEPKcr+oJrhUUXYV6SsdD7diUtuH9hM86U5PE=", new byte[] { 137, 236, 71, 98, 96, 13, 158, 202, 31, 27, 227, 208, 114, 118, 225, 96, 224, 209, 135, 219, 122, 75, 226, 152, 194, 145, 202, 134, 8, 137, 219, 94 } });
        }
    }
}
