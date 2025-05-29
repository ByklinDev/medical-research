using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class MedicineTypeService(IUnitOfWork unitOfWork, IValidator<MedicineType> medicineTypeValidator, ILogger<MedicineTypeService> logger) : IMedicineTypeService
    {
        public async Task<MedicineType> AddMedicineTypeAsync(MedicineType medicineType)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine type {medicineType} could not be added: {message}", medicineType.Name, ex.Message);
                throw new DomainException($"Error while adding Medicine type {medicineType.Name}");
            }
        }

        public async Task<bool> DeleteMedicineTypeAsync(int id)
        {
            try
            {
                var existedMedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine type no found.");
                var isDelete = unitOfWork.MedicineTypeRepository.Delete(existedMedicineType);
                return await unitOfWork.SaveAsync() > 0 ? isDelete : throw new DomainException("Medicine type not deleted.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine type with id {id} could not be deleted: {message}", id, ex.Message);
                throw new DomainException($"Error while deleting Medicine type with id {id}");
            }
        }

        public async Task<MedicineType?> GetMedicineTypeAsync(int id)
        {
            try
            {
                return await unitOfWork.MedicineTypeRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine type with id {id} could not be retrieved: {message}", id, ex.Message);
                throw new DomainException($"Error while retrieving Medicine type with id {id}");
            }
        }

        public async Task<List<MedicineType>> GetMedicineTypesAsync(Query query)
        {
            try
            {
                return await unitOfWork.MedicineTypeRepository.GetAllAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while retrieving medicine types: {message}", ex.Message);
                throw new DomainException("Error while retrieving medicine types");
            }
        }

        public async Task<MedicineType> UpdateMedicineTypeAsync(MedicineType medicineType)
        {
            try 
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine type {medicineType} could not be updated: {message}", medicineType.Name, ex.Message);
                throw new DomainException($"Error while updating Medicine type {medicineType.Name}");
            }
        }
        public async Task<List<MedicineType>> GetMedicineTypesByNameAsync(Query query)
        {
            try
            {
                return await unitOfWork.MedicineTypeRepository.SearchByTermAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while searching medicine types by name: {message}", ex.Message);
                throw new DomainException("Error while searching medicine types by name");
            }
        }

    }
}