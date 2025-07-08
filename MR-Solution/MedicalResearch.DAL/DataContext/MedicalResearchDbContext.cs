using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Utilites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace MedicalResearch.DAL.DataContext
{
    public class MedicalResearchDbContext(DbContextOptions<MedicalResearchDbContext> options): DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicStockMedicine> ClinicsStockMedicines { get; set; }
        public DbSet<MedicineContainer> MedicineContainers { get; set; }
        public DbSet<DosageForm> DosageForms { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineType> MedicinesTypes { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
/*            modelBuilder.Entity<User>().UseTpcMappingStrategy().ToTable("Users");
            modelBuilder.Entity<Role>().UseTpcMappingStrategy().ToTable("Roles");
            modelBuilder.Entity<Clinic>().UseTpcMappingStrategy().ToTable("Clinics");
            modelBuilder.Entity<ClinicStockMedicine>().UseTpcMappingStrategy().ToTable("ClinicsStockMedicines");
            modelBuilder.Entity<MedicineContainer>().UseTpcMappingStrategy().ToTable("MedicinesContainers");
            modelBuilder.Entity<DosageForm>().UseTpcMappingStrategy().ToTable("DosageForms");
            modelBuilder.Entity<Medicine>().UseTpcMappingStrategy().ToTable("Medicines");
            modelBuilder.Entity<MedicineType>().UseTpcMappingStrategy().ToTable("MedicinesTypes");
            modelBuilder.Entity<Patient>().UseTpcMappingStrategy().ToTable("Patients");
            modelBuilder.Entity<Supply>().UseTpcMappingStrategy().ToTable("Supplies");
            modelBuilder.Entity<Visit>().UseTpcMappingStrategy().ToTable("Visits");
*/


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<Clinic>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<MedicineContainer>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<MedicineType>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<DosageForm>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Patient>().HasIndex(p => p.Number).IsUnique();

            modelBuilder.Entity<Role>().HasData(new Role() { Id = 1, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 2, Name = "Sponsor" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 3, Name = "Researcher" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 4, Name = "Manager" });

            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 1, Name = "A" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 2, Name = "B" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 3, Name = "C" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 4, Name = "D" });

            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 1, Name = "tablet" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 2, Name = "capsule" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 3, Name = "syrup" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 4, Name = "suspension" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 5, Name = "oinment" });

            modelBuilder.Entity<MedicineContainer>().HasData(new MedicineContainer() { Id = 1, Name = "box" });
            modelBuilder.Entity<MedicineContainer>().HasData(new MedicineContainer() { Id = 2, Name = "bottle" });
            modelBuilder.Entity<MedicineContainer>().HasData(new MedicineContainer() { Id = 3, Name = "blister" });
            modelBuilder.Entity<MedicineContainer>().HasData(new MedicineContainer() { Id = 4, Name = "ampoule" });
            modelBuilder.Entity<MedicineContainer>().HasData(new MedicineContainer() { Id = 5, Name = "vial" });

            var salt = SecurePassword.GenerateSalt();
            var hmac = SecurePassword.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes("admin635"), salt);
            
            modelBuilder.Entity<User>().HasData(new User() { Id = 1, FirstName = "David", LastName = "Duchovny", Email = "byklin@list.ru", PasswordSalt = salt, Password = Convert.ToBase64String(hmac) });

            modelBuilder.Entity<User>().HasMany(p => p.Roles).WithMany(s => s.Users)
                .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                l => l
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                jl =>
                {
                    jl.HasKey("UserId", "RoleId").HasName("PK_UserRole");
                    jl.HasData(new { UserId = 1, RoleId = 1 });
                }
                );
        }
    }
}