using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class DosageFormService(IUnitOfWork unitOfWork, IValidator<DosageForm> dosageFormValidator, ILogger<DosageFormService> logger) : IDosageFormService
{

    public async Task<DosageForm> AddDosageFormAsync(DosageForm dosageForm)
    {
        DosageForm? added;
        int countAdded;

        var validationResult = await dosageFormValidator.ValidateAsync(dosageForm);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }
        var existingDosageForm = await unitOfWork.DosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name);
        if (existingDosageForm != null)
        {
            throw new DomainException("Dosage form already exists");
        }

        try
        {
            added = await unitOfWork.DosageFormRepository.AddAsync(dosageForm);
            countAdded = await unitOfWork.SaveAsync(); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dosage form {dosageForm} could not be added: {message}", dosageForm.Name, ex.Message);
            throw new DomainException($"Error while adding Dosage form {dosageForm.Name}");
        }
        return countAdded > 0  && added != null ? added : throw new DomainException("Dosage form not added");
    }

    public async Task<bool> DeleteDosageFormAsync(int id)
    {
        var dosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(id) ?? throw new DomainException("Dosage form not found");
        try
        {
            var result = unitOfWork.DosageFormRepository.Delete(dosageForm);
            return result && await unitOfWork.SaveAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dosage form with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Dosage form with id {id}");
        }
    }

    public async Task<DosageForm?> GetDosageFormAsync(int id)
    {
        try
        {
            return await unitOfWork.DosageFormRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dosage form with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Dosage form with id {id}");
        }
    }

    public async Task<PagedList<DosageForm>> GetDosageFormsAsync(Query query)
    {
        try
        {
            return await unitOfWork.DosageFormRepository.SearchByTermAsync(query); ;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dosage forms could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving dosage forms");
        }
    }

    public async Task<DosageForm> UpdateDosageFormAsync(DosageForm dosageForm)
    {
        DosageForm? updated;
        int countUpdated;
        var validationResult = await dosageFormValidator.ValidateAsync(dosageForm);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }
        var existingDosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(dosageForm.Id) ?? throw new DomainException("Dosage form not found");
        var existingDosageFormWithSameName = await unitOfWork.DosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name);
        if (existingDosageFormWithSameName != null && existingDosageFormWithSameName.Id != dosageForm.Id)
        {
            throw new DomainException("Dosage form with the same name already exists");
        }
        try
        {  
            existingDosageForm.Name = dosageForm.Name;
            updated = unitOfWork.DosageFormRepository.Update(existingDosageForm);
            countUpdated = await unitOfWork.SaveAsync();      
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dosage form {dosageForm} could not be updated: {message}", dosageForm.Name, ex.Message);
            throw new DomainException($"Error while updating Dosage form {dosageForm.Name}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Dosage form not updated");
    }      
}
