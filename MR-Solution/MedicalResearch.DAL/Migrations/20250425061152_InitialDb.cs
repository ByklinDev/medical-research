using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalResearch.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    AddressOne = table.Column<string>(type: "text", nullable: false),
                    AddressTwo = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DosageForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosageForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineContainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicinesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => new { x.Id, x.ClinicId });
                    table.ForeignKey(
                        name: "FK_Patients_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Initials = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PaswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    ClinicId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    MedicineTypeId = table.Column<int>(type: "integer", nullable: false),
                    ContainerId = table.Column<int>(type: "integer", nullable: false),
                    DosageFormId = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_DosageForms_DosageFormId",
                        column: x => x.DosageFormId,
                        principalTable: "DosageForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineContainers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "MedicineContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicinesTypes_MedicineTypeId",
                        column: x => x.MedicineTypeId,
                        principalTable: "MedicinesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicsStockMedicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<int>(type: "integer", nullable: false),
                    MedicineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicsStockMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicsStockMedicines_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicsStockMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<int>(type: "integer", nullable: false),
                    MedicineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supplies_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    PatientClinicId = table.Column<int>(type: "integer", nullable: false),
                    DateOfVisit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MedicineId = table.Column<int>(type: "integer", nullable: false),
                    NumberOfVisit = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visits_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visits_Patients_PatientId_PatientClinicId",
                        columns: x => new { x.PatientId, x.PatientClinicId },
                        principalTable: "Patients",
                        principalColumns: new[] { "Id", "ClinicId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicStockMedicineSupply",
                columns: table => new
                {
                    ClinicStockMedicinesId = table.Column<int>(type: "integer", nullable: false),
                    SuppliesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicStockMedicineSupply", x => new { x.ClinicStockMedicinesId, x.SuppliesId });
                    table.ForeignKey(
                        name: "FK_ClinicStockMedicineSupply_ClinicsStockMedicines_ClinicStock~",
                        column: x => x.ClinicStockMedicinesId,
                        principalTable: "ClinicsStockMedicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicStockMedicineSupply_Supplies_SuppliesId",
                        column: x => x.SuppliesId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DosageForms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "tablet" },
                    { 2, "capsule" },
                    { 3, "syrup" },
                    { 4, "suspension" },
                    { 5, "oinment" }
                });

            migrationBuilder.InsertData(
                table: "MedicineContainers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "box" },
                    { 2, "bottle" },
                    { 3, "blister" },
                    { 4, "ampoule" },
                    { 5, "vial" }
                });

            migrationBuilder.InsertData(
                table: "MedicinesTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "A" },
                    { 2, "B" },
                    { 3, "C" },
                    { 4, "D" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Sponsor" },
                    { 3, "Researcher" },
                    { 4, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ClinicId", "Email", "FirstName", "Initials", "LastName", "Password", "PaswordSalt", "State" },
                values: new object[] { 1, null, "byklin@list.ru", "Admin", "", "", "ma+sYwb8QHKHMsfJtQ3l3sLZ4OfAKd7dtnxOAP/KCTA=", new byte[] { 84, 16, 42, 85, 56, 114, 88, 212, 43, 27, 186, 97, 62, 131, 73, 15, 91, 164, 87, 147, 228, 98, 236, 141, 140, 237, 150, 238, 184, 53, 251, 139 }, 0 });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_Name",
                table: "Clinics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicsStockMedicines_ClinicId",
                table: "ClinicsStockMedicines",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicsStockMedicines_MedicineId",
                table: "ClinicsStockMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStockMedicineSupply_SuppliesId",
                table: "ClinicStockMedicineSupply",
                column: "SuppliesId");

            migrationBuilder.CreateIndex(
                name: "IX_DosageForms_Name",
                table: "DosageForms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineContainers_Name",
                table: "MedicineContainers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_ContainerId",
                table: "Medicines",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_DosageFormId",
                table: "Medicines",
                column: "DosageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineTypeId",
                table: "Medicines",
                column: "MedicineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinesTypes_Name",
                table: "MedicinesTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClinicId",
                table: "Patients",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_ClinicId",
                table: "Supplies",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_MedicineId",
                table: "Supplies",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClinicId",
                table: "Users",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_ClinicId",
                table: "Visits",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_MedicineId",
                table: "Visits",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientId_PatientClinicId",
                table: "Visits",
                columns: new[] { "PatientId", "PatientClinicId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_UserId",
                table: "Visits",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicStockMedicineSupply");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "ClinicsStockMedicines");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "DosageForms");

            migrationBuilder.DropTable(
                name: "MedicineContainers");

            migrationBuilder.DropTable(
                name: "MedicinesTypes");
        }
    }
}
