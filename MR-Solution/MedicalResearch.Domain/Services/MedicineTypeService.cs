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
    public class MedicineTypeService(IMedicineTypeRepository medicineTypeRepository, IValidator<MedicineType> medicineTypeValidator) : IMedicineTypeService
    {
        public async Task<MedicineType> AddMedicineTypeAsync(MedicineType medicineType)
        {
            var validationResult = await medicineTypeValidator.ValidateAsync(medicineType);
            if (!validationResult.IsValid) 
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existedMedicineType = await medicineTypeRepository.GetMedicineTypeByNameAsync(medicineType.Name);
            if (existedMedicineType != null) 
            {
                throw new DomainException("Medicine type already exists.");
            }
            return await medicineTypeRepository.AddAsync(medicineType);
        }

        public async Task<bool> DeleteMedicineTypeAsync(int id)
        {
            var existedMedicineType = await medicineTypeRepository.GetMedicineTypeByIdAsync(id) ?? throw new DomainException("Medicine type no found."); 
            return await medicineTypeRepository.DeleteAsync(existedMedicineType);
        }

        public async Task<MedicineType?> GetMedicineTypeAsync(int id)
        {
            return await medicineTypeRepository.GetMedicineTypeByIdAsync(id);
        }

        public async Task<List<MedicineType>> GetMedicineTypesAsync()
        {
            return await medicineTypeRepository.GetAllAsync();
        }

        public async Task<MedicineType> UpdateMedicineTypeAsync(MedicineType medicineType)
        {
            var existedMedicineType = await medicineTypeRepository.GetMedicineTypeByIdAsync(medicineType.Id) ?? throw new DomainException("Medicine type no found.");
            var validationResult = await medicineTypeValidator.ValidateAsync(medicineType);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existedMedicineTypeWithSameName = await medicineTypeRepository.GetMedicineTypeByNameAsync(medicineType.Name);
            if (existedMedicineTypeWithSameName != null && existedMedicineTypeWithSameName.Id != medicineType.Id)
            {
                throw new DomainException("Medicine type with this name already exists.");
            }
            existedMedicineType.Name = medicineType.Name;
            return await medicineTypeRepository.UpdateAsync(existedMedicineType);
        }
    }
}