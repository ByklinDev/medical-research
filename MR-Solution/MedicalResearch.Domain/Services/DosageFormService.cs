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
    public class DosageFormService(IUnitOfWork unitOfWork, IValidator<DosageForm> dosageFormValidator, ILogger<DosageFormService> logger) : IDosageFormService
    {
        public async Task<DosageForm> AddDosageFormAsync(DosageForm dosageForm)
        {
            try
            {
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
                var added = await unitOfWork.DosageFormRepository.AddAsync(dosageForm);
                return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Dosage form not added");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Dosage form {dosageForm} could not be added: {message}", dosageForm.Name, ex.Message);
                throw new DomainException($"Error while adding Dosage form {dosageForm.Name}");
            }
        }

        public async Task<bool> DeleteDosageFormAsync(int id)
        {
            try
            {
                var dosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(id) ?? throw new DomainException("Dosage form not found");
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

        public async Task<List<DosageForm>> GetDosageFormsAsync(Query query)
        {
            try
            {
                return await unitOfWork.DosageFormRepository.GetAllAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Dosage forms could not be retrieved: {message}", ex.Message);
                throw new DomainException("Error while retrieving dosage forms");
            }
        }

        public async Task<DosageForm> UpdateDosageFormAsync(DosageForm dosageForm)
        {
            try
            {
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
                existingDosageForm.Name = dosageForm.Name;
                var updated = unitOfWork.DosageFormRepository.Update(existingDosageForm);
                return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Dosage form not updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Dosage form {dosageForm} could not be updated: {message}", dosageForm.Name, ex.Message);
                throw new DomainException($"Error while updating Dosage form {dosageForm.Name}");
            }
        }

        public async Task<List<DosageForm>> GetDosageFormsByNameAsync(Query query)
        {
            try
            {
                return await unitOfWork.DosageFormRepository.SearchByTermAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Dosage forms by name could not be retrieved: {message}", ex.Message);
                throw new DomainException("Error while retrieving dosage forms by name");
            }
        }
    }
}
