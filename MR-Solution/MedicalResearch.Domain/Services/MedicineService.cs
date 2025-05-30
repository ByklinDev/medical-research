using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class MedicineService(IUnitOfWork unitOfWork, IValidator<Medicine> medicineValidator, ILogger<MedicineService> logger) : IMedicineService
{
    public async Task<Medicine> AddMedicineAsync(Medicine medicine)
    {
        try
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
        catch (DomainException ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be added: {message}", medicine.Description, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be added: {message}", medicine.Description, ex.Message);
            throw new DomainException($"Error while adding Medicine {medicine.Description}");
        }
    }

    public async Task<bool> DeleteMedicineAsync(int id)
    {
        try
        {
            var medicine = await unitOfWork.MedicineRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine not found");
            var isDelete = unitOfWork.MedicineRepository.Delete(medicine);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Medicine with id {id} could not be deleted: {message}", id, ex.Message);
            throw;
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

    public async Task<List<Medicine>> GetMedicinesAsync(Query query)
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
        try 
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
            var updated = unitOfWork.MedicineRepository.Update(existingMedicine);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Medicine not updated");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be updated: {message}", medicine.Description, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Medicine {medicine} could not be updated: {message}", medicine.Description, ex.Message);
            throw new DomainException($"Error while updating Medicine {medicine.Description}");
        }
    }
}
