using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class MedicineService(IUnitOfWork unitOfWork, IValidator<Medicine> medicineValidator, ILogger<MedicineService> logger) : IMedicineService
{
    public async Task<Medicine> AddMedicineAsync(Medicine medicine)
    {
        Medicine? added;
        int countAdded;
        medicine.State = Enums.MedicineState.Ok;
        medicine.ExpireAt = medicine.ExpireAt.ToUniversalTime();
        medicine.MedicineContainer = await unitOfWork.MedicineContainerRepository.GetByIdAsync(medicine.MedicineContainerId);
        medicine.DosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(medicine.DosageFormId);
        medicine.MedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(medicine.MedicineTypeId);

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
        try
        {
            added = await unitOfWork.MedicineRepository.AddAsync(medicine);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be added: {message}", medicine.Description, ex.Message);
            throw new DomainException($"Error while adding Medicine {medicine.Description}");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("Medicine not added");
    }

    public async Task<bool> DeleteMedicineAsync(int id)
    {
        var medicine = await unitOfWork.MedicineRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine not found");

        try
        {
            var isDelete = unitOfWork.MedicineRepository.Delete(medicine);
            return isDelete && await unitOfWork.SaveAsync() > 0;       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Medicine with id {id}");
        }
    }

    public async Task<Medicine?> GetMedicineAsync(int id)
    {
        try
        {
            return await unitOfWork.MedicineRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Medicine with id {id}");
        }
    }

    public async Task<PagedList<Medicine>> GetMedicinesAsync(Query query)
    {
        try
        {
            return await unitOfWork.MedicineRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicines could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving Medicines");
        }
    }

    public async Task<Medicine> UpdateMedicineAsync(Medicine medicine)
    {
        Medicine? updated;
        int countUpdated;

        var existingMedicine = await unitOfWork.MedicineRepository.GetByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");

        existingMedicine.Description = medicine.Description;
        existingMedicine.ExpireAt = medicine.ExpireAt.ToUniversalTime();
        existingMedicine.Amount = medicine.Amount;
        existingMedicine.MedicineTypeId = medicine.MedicineTypeId;
        existingMedicine.MedicineContainerId = medicine.MedicineContainerId;
        existingMedicine.DosageFormId = medicine.DosageFormId;
        existingMedicine.State = medicine.State;

        existingMedicine.DosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(medicine.DosageFormId);
        existingMedicine.MedicineContainer = await unitOfWork.MedicineContainerRepository.GetByIdAsync(medicine.MedicineContainerId);
        existingMedicine.MedicineType = await unitOfWork.MedicineTypeRepository.GetByIdAsync(medicine.MedicineTypeId);

        var validationResult = await medicineValidator.ValidateAsync(existingMedicine);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }

        try
        {
            updated = unitOfWork.MedicineRepository.Update(existingMedicine);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be updated: {message}", medicine.Description, ex.Message);
            throw new DomainException($"Error while updating Medicine {medicine.Description}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Medicine not updated");
    }
}
