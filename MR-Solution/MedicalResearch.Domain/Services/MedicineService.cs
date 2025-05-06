using FluentValidation;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class MedicineService(IMedicineRepository medicineRepository, IValidator<Medicine> medicineValidator) : IMedicineService
    {
        public async Task<Medicine> AddMedicineAsync(Medicine medicine)
        {
            var validationResult = await medicineValidator.ValidateAsync(medicine);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicine = await medicineRepository.GetMedicineAsync(medicine);
            if (existingMedicine != null)
            {
                throw new DomainException("Medicine already exists");
            }
            return await medicineRepository.AddAsync(medicine);
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            var medicine = await medicineRepository.GetMedicineByIdAsync(id) ?? throw new DomainException("Medicine not found");
            return await medicineRepository.DeleteAsync(medicine);
        }

        public async Task<Medicine?> GetMedicineAsync(int id)
        {
            return await medicineRepository.GetMedicineByIdAsync(id);
        }

        public async Task<List<Medicine>> GetMedicinesAsync()
        {
            return await medicineRepository.GetAllAsync();
        }

        public async Task<Medicine> UpdateMedicineAsync(Medicine medicine)
        {
            var validationResult = await medicineValidator.ValidateAsync(medicine);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicine = await medicineRepository.GetMedicineByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");
            existingMedicine.Description = medicine.Description;
            existingMedicine.ExpireAt = medicine.ExpireAt;
            existingMedicine.Amount = medicine.Amount;
            existingMedicine.MedicineTypeId = medicine.MedicineTypeId;
            existingMedicine.MedicineContainerId = medicine.MedicineContainerId;
            existingMedicine.DosageFormId = medicine.DosageFormId;
            existingMedicine.State = medicine.State;
            return await medicineRepository.UpdateAsync(existingMedicine);
        }
    }
}
