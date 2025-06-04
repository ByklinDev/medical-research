using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class ClinicStockMedicineService(IUnitOfWork unitOfWork, ILogger<ClinicStockMedicineService> logger) : IClinicStockMedicineService
{

    public async Task<ClinicStockMedicine?> GetClinicStockMedicineByIdAsync(int id)
    {
        try
        {
            return await unitOfWork.ClinicStockMedicineRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving ClinicStockMedicine with id {id}: {message}", id, ex.Message);
            throw new DomainException($"Error retrieving ClinicStockMedicine with id {id}: {ex.Message}");
        }
    }

    public async Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId)
    {
        try
        {
            return await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(clinicId, medicineId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving ClinicStockMedicine for ClinicId {clinicId} and MedicineId {medicineId}: {message}", clinicId, medicineId, ex.Message);
            throw new DomainException($"Error retrieving ClinicStockMedicine for ClinicId {clinicId} and MedicineId {medicineId}: {ex.Message}");
        }
    }

    public async Task<PagedList<ClinicStockMedicine>> GetClinicStockMedicinesByClinicIdAsync(int clinicId, Query query)
    {
        try
        {
            return await unitOfWork.ClinicStockMedicineRepository.SearchByTermAsync(clinicId, query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving ClinicStockMedicines for ClinicId {clinicId}: {message}", clinicId, ex.Message);
            throw new DomainException($"Error retrieving ClinicStockMedicines for ClinicId {clinicId}: {ex.Message}");
        }
    }

    public async Task<PagedList<ClinicStockMedicine>> GetClinicStockMedicinesAsync(Query query)
    {
        try
        {
            return await unitOfWork.ClinicStockMedicineRepository.SearchByTermAsync(null, query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving ClinicStockMedicines: {message}", ex.Message);
            throw new DomainException($"Error retrieving ClinicStockMedicines: {ex.Message}");
        }
    }
}
