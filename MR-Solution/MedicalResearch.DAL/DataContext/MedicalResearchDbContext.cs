using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Utilites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace MedicalResearch.DAL.DataContext
{
    public class MedicalResearchDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicStock> ClinicsStocks { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<DosageForm> DosageForms { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineType> MedicinesTypes { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Visit> Visits { get; set; }


        public MedicalResearchDbContext(DbContextOptions<MedicalResearchDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<Patient>().HasKey(p => new { p.Id, p.ClinicId });
            modelBuilder.Entity<Visit>().HasKey(p => new { p.PatientId, p.ClinicId, p.DateOfVisit, p.MedicineId });

            modelBuilder.Entity<Role>().HasData(new Role() { Id = 1, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 2, Name = "Sponsor" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 3, Name = "Researcher" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 4, Name = "Manager" });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 5, Name = "Anonymous" });

            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 1, Name = "A" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 2, Name = "B" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 3, Name = "C" });
            modelBuilder.Entity<MedicineType>().HasData(new MedicineType() { Id = 4, Name = "D" });

            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 1, Name = "tablet" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 2, Name = "capsule" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 3, Name = "syrup" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 4, Name = "suspension" });
            modelBuilder.Entity<DosageForm>().HasData(new DosageForm() { Id = 5, Name = "oinment" });

            modelBuilder.Entity<Container>().HasData(new Container() { Id = 1, Name = "box" });
            modelBuilder.Entity<Container>().HasData(new Container() { Id = 2, Name = "bottle" });
            modelBuilder.Entity<Container>().HasData(new Container() { Id = 3, Name = "blister" });
            modelBuilder.Entity<Container>().HasData(new Container() { Id = 4, Name = "ampoule" });
            modelBuilder.Entity<Container>().HasData(new Container() { Id = 5, Name = "vial" });

            var salt = SecurePassword.GenerateSalt();
            var hmac = SecurePassword.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes("admin635"), salt);
            modelBuilder.Entity<User>().HasData(new User() { Id = 1, FirstName = "Admin", Email = "byklin@list.ru", Password = Convert.ToBase64String(hmac) });

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

