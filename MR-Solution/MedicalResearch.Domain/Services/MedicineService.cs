using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
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
    public class MedicineService(IUnitOfWork unitOfWork, IValidator<Medicine> medicineValidator) : IMedicineService
    {
        public async Task<Medicine> AddMedicineAsync(Medicine medicine)
        {
            var validationResult = await medicineValidator.ValidateAsync(medicine);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicine = await unitOfWork.MedicineRepository.GetMedicineAsync(medicine);
            if (existingMedicine != null)
            {
                throw new DomainException("Same medicine already exists");
            }
            var added = await unitOfWork.MedicineRepository.AddAsync(medicine);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Medicine not added");
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            var medicine = await unitOfWork.MedicineRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine not found");
            var isDelete = unitOfWork.MedicineRepository.Delete(medicine);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }

        public async Task<Medicine?> GetMedicineAsync(int id)
        {
            return await unitOfWork.MedicineRepository.GetByIdAsync(id);
        }

        public async Task<List<Medicine>> GetMedicinesAsync()
        {
            return await unitOfWork.MedicineRepository.GetAllAsync();
        }

        public async Task<Medicine> UpdateMedicineAsync(Medicine medicine)
        {
            var validationResult = await medicineValidator.ValidateAsync(medicine);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicine = await unitOfWork.MedicineRepository.GetByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");
            existingMedicine.Description = medicine.Description;
            existingMedicine.ExpireAt = medicine.ExpireAt;
            existingMedicine.Amount = medicine.Amount;
            existingMedicine.MedicineTypeId = medicine.MedicineTypeId;
            existingMedicine.MedicineContainerId = medicine.MedicineContainerId;
            existingMedicine.DosageFormId = medicine.DosageFormId;
            existingMedicine.State = medicine.State;
            var updated =  unitOfWork.MedicineRepository.Update(existingMedicine);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Medicine not updated");
        }
    }
}
