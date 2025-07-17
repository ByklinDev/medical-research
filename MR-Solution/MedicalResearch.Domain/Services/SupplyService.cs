using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class SupplyService(IUnitOfWork unitOfWork, IValidator<Supply> validator, ILogger<SupplyService> logger) : ISupplyService
{
    public async Task<List<Supply>> AddSupplyAsync(List<Supply> supplies, int userId)
    {
        List<Supply> addedSupplies = [];
        foreach (var sup in supplies)
        {
            var supply = await unitOfWork.SupplyRepository.GetSupplyByIdAsync(sup.Id) ?? throw new DomainException("Supply not found for id: " + sup.Id);
            if (supply.IsActive == true)
            {
                throw new DomainException("Supply cannot be active when adding to supply list");
            }
            if (supply.UserId != userId)
            {
                throw new DomainException("You cannot add supplies that you did not create");
            }
//            var medicineToSupply = await unitOfWork.MedicineRepository.GetByIdAsync(supply.MedicineId) ?? throw new DomainException("Medicine not found");
            if (supply.Medicine.Amount < supply.Amount)
            {
                throw new DomainException("Not enough medicine in stock");
            }
            else
            {
                supply.Medicine.Amount -= supply.Amount;
                //unitOfWork.MedicineRepository.Update(medicineToSupply);
            }
            supply.IsActive = true;
            
            var addedSupply = unitOfWork.SupplyRepository.Update(supply) ?? throw new DomainException("Supply could not be updated");

            var clinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(supply.MedicineId, supply.ClinicId);
            if (clinicStockMedicine != null)
            {
                clinicStockMedicine.Amount += supply.Amount;
                unitOfWork.ClinicStockMedicineRepository.Update(clinicStockMedicine);
            }
            else
            {
                var newClinicStockMedicine = new ClinicStockMedicine
                {
                    MedicineId = supply.MedicineId,
                    ClinicId = supply.ClinicId,
                    Amount = supply.Amount,
                    Clinic = supply.Clinic,
                    Medicine = supply.Medicine,
                };
                await unitOfWork.ClinicStockMedicineRepository.AddAsync(newClinicStockMedicine);
            }
            addedSupplies.Add(addedSupply);
        }
        try
        {
            await unitOfWork.SaveAsync();
            return addedSupplies;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Supplies could not be added: {message}", ex.Message);
            throw new DomainException("Error while adding supplies");
        }
    }

    public async Task<Supply> AddToSupply(Medicine medicine, int amount, int clinicId, int userId)
    {
        Supply? added;
        int countAdded;

        var medicineToSupply = await unitOfWork.MedicineRepository.GetByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");
        if (medicineToSupply.Amount < amount)
        {
            throw new DomainException("Not enough medicine in stock");
        }
        var supply = new Supply
        {
            MedicineId = medicine.Id,
            Amount = amount,
            ClinicId = clinicId,
            DateArrival = DateTime.UtcNow,
            IsActive = false,
            UserId = userId
        };
        var clinic = await unitOfWork.ClinicRepository.GetByIdAsync(clinicId);
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user != null)
        {
            supply.User = user;
        }
        if (clinic != null)
        {
            supply.Clinic = clinic;
        }

        supply.Medicine = medicine;

        var resultValidation = await validator.ValidateAsync(supply);
        if (!resultValidation.IsValid)
        {
            throw new DomainException(resultValidation.Errors.First().ErrorMessage);
        }
        try
        {
            added = await unitOfWork.SupplyRepository.AddAsync(supply);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding to supply: {message}", ex.Message);
            throw new DomainException("Error while adding to supply");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("Supply not added");
    }






    public async Task<bool> DeleteSupplyAsync(int id)
    {
        int countDeleted = 0;

        var existingSupply = await unitOfWork.SupplyRepository.GetByIdAsync(id) ?? throw new DomainException("Supply not found");
        if (existingSupply.IsActive == true)
        {
            var medicine = await unitOfWork.MedicineRepository.GetByIdAsync(existingSupply.MedicineId) ?? throw new DomainException("Medicine not found");
            var clinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
            if (clinicStockMedicine == null)
            {
                throw new DomainException("Clinic stock medicine not found");
            }
            try
            {
                medicine.Amount += existingSupply.Amount;
                unitOfWork.MedicineRepository.Update(medicine);

                clinicStockMedicine.Amount -= existingSupply.Amount;
                unitOfWork.ClinicStockMedicineRepository.Update(clinicStockMedicine);

                var isDelete = unitOfWork.SupplyRepository.Delete(existingSupply);
                if (isDelete)
                {
                    countDeleted = await unitOfWork.SaveAsync();
                }
                return countDeleted > 0 && isDelete == true ? isDelete : throw new DomainException("Supply not deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Supply with id {id} could not be deleted: {message}", id, ex.Message);
                throw new DomainException($"Error while deleting Supply with id {id}");
            }
        }
        else
        {
            try
            {
                var isDelete = unitOfWork.SupplyRepository.Delete(existingSupply);
                if (isDelete)
                {
                    countDeleted = await unitOfWork.SaveAsync();
                }
                return countDeleted > 0 && isDelete == true ? isDelete : throw new DomainException("Supply not deleted");

            }catch (Exception ex)
            {
                logger.LogError(ex, "Supply with id {id} could not be deleted: {message}", id, ex.Message);
                throw new DomainException($"Error while deleting Supply with id {id}");
            }
        }

    }

    public async Task<PagedList<Supply>> GetSuppliesAsync(int? clinicId, int? medicineId, Query query)
    {
        try
        {
            return await unitOfWork.SupplyRepository.SearchByTermAsync(clinicId, medicineId, query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving supplies: {message}", ex.Message);
            throw new DomainException("Error while retrieving supplies");
        }
    }

    public async Task<PagedList<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query)
    {
        try
        {
            return await unitOfWork.SupplyRepository.GetInactiveSuppliesByUserIdAsync(userId, query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving supplies: {message}", ex.Message);
            throw new DomainException("Error while retrieving supplies");
        }
    }


    public async Task<Supply?> GetSupplyAsync(int id)
    {
        try
        {
            return await unitOfWork.SupplyRepository.GetSupplyByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving supply with id {id}: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving supply with id {id}");
        }
    }

    public async Task<Supply> UpdateSupplyAsync(Supply supply)
    {
        Supply? updated;
        int countUpdated;

        var resultValidation = await validator.ValidateAsync(supply);
        if (!resultValidation.IsValid)
        {
            throw new DomainException(resultValidation.Errors.First().ErrorMessage);
        }
        var existingSupply = await unitOfWork.SupplyRepository.GetByIdAsync(supply.Id) ?? throw new DomainException("Supply not found");
        var diff = supply.Amount - existingSupply.Amount;
        var existingClinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
        if (existingClinicStockMedicine == null)
        {
            throw new DomainException("Clinic stock medicine not found");
        }
        if (existingClinicStockMedicine.Amount + diff < 0)
        {
            throw new DomainException("Not enough medicine in stock");
        }

        try
        {
            existingClinicStockMedicine.Amount += diff;
            existingSupply.Amount = supply.Amount;
            unitOfWork.ClinicStockMedicineRepository.Update(existingClinicStockMedicine);
            updated = unitOfWork.SupplyRepository.Update(existingSupply);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating supply: {message}", ex.Message);
            throw new DomainException("Error while updating supply");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Supply not updated");
    }
}