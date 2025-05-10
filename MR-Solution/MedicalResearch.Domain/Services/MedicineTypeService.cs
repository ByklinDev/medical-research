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
    public class MedicineTypeService(IUnitOfWork unitOfWork, IValidator<MedicineType> medicineTypeValidator) : IMedicineTypeService
    {
        public async Task<MedicineType> AddMedicineTypeAsync(MedicineType medicineType)
        {
            var validationResult = await medicineTypeValidator.ValidateAsync(medicineType);
            if (!validationResult.IsValid) 
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existedMedicineType = await unitOfWork.MedicineTypeRepository.GetMedicineTypeByNameAsync(medicineType.Name);
            if (existedMedicineType != null) 
            {
                throw new DomainException("Medicine type already exists.");
            }
            var added = await unitOfWork.MedicineTypeRepository.AddAsync(medicineType);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Medicine type not added.");
        }

        public async Task<bool> DeleteMedicineTypeAsync(int id)
        {
            var existedMedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine type no found."); 
            var isDelete = unitOfWork.MedicineTypeRepository.Delete(existedMedicineType);
            return await unitOfWork.SaveAsync() > 0 ? isDelete : throw new DomainException("Medicine type not deleted.");
        }

        public async Task<MedicineType?> GetMedicineTypeAsync(int id)
        {
            return await unitOfWork.MedicineTypeRepository.GetByIdAsync(id);
        }

        public async Task<List<MedicineType>> GetMedicineTypesAsync()
        {
            return await unitOfWork.MedicineTypeRepository.GetAllAsync();
        }

        public async Task<MedicineType> UpdateMedicineTypeAsync(MedicineType medicineType)
        {
            var existedMedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(medicineType.Id) ?? throw new DomainException("Medicine type no found.");
            var validationResult = await medicineTypeValidator.ValidateAsync(medicineType);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existedMedicineTypeWithSameName = await unitOfWork.MedicineTypeRepository.GetMedicineTypeByNameAsync(medicineType.Name);
            if (existedMedicineTypeWithSameName != null && existedMedicineTypeWithSameName.Id != medicineType.Id)
            {
                throw new DomainException("Medicine type with this name already exists.");
            }
            existedMedicineType.Name = medicineType.Name;
            var updated = unitOfWork.MedicineTypeRepository.Update(existedMedicineType);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Medicine type not updated.");
        }
    }
}