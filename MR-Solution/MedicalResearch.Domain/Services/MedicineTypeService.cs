using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class MedicineTypeService(IUnitOfWork unitOfWork, IValidator<MedicineType> medicineTypeValidator, ILogger<MedicineTypeService> logger) : IMedicineTypeService
{
    public async Task<MedicineType> AddMedicineTypeAsync(MedicineType medicineType)
    {
        MedicineType? added;
        int countAdded;

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

        try
        {
            added = await unitOfWork.MedicineTypeRepository.AddAsync(medicineType);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine type {medicineType} could not be added: {message}", medicineType.Name, ex.Message);
            throw new DomainException($"Error while adding Medicine type {medicineType.Name}");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("Medicine type not added.");
    }

    public async Task<bool> DeleteMedicineTypeAsync(int id)
    {
        var existedMedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine type no found.");
        try
        {
            var isDelete = unitOfWork.MedicineTypeRepository.Delete(existedMedicineType);
            return await unitOfWork.SaveAsync() > 0 && isDelete;        
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

    public async Task<PagedList<MedicineType>> GetMedicineTypesAsync(Query query)
    {
        try
        {
            return await unitOfWork.MedicineTypeRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving medicine types: {message}", ex.Message);
            throw new DomainException("Error while retrieving medicine types");
        }
    }

    public async Task<MedicineType> GetRandomMedicineTypesAsync()
    {
        try
        {
            return await unitOfWork.MedicineTypeRepository.GetRandomMedicineTypeAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving medicine types: {message}", ex.Message);
            throw new DomainException("Error while retrieving medicine types");
        }
    }

    public async Task<MedicineType> UpdateMedicineTypeAsync(MedicineType medicineType)
    {
        MedicineType? updated;
        int countUpdated;

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
        try
        {
            existedMedicineType.Name = medicineType.Name;
            updated = unitOfWork.MedicineTypeRepository.Update(existedMedicineType);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine type {medicineType} could not be updated: {message}", medicineType.Name, ex.Message);
            throw new DomainException($"Error while updating Medicine type {medicineType.Name}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Medicine type not updated.");
    }
}