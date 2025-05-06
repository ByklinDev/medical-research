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
    public class DosageFormService(IDosageFormRepository dosageFormRepository, IValidator<DosageForm> dosageFormValidator) : IDosageFormService
    {
        public async Task<DosageForm> AddDosageFormAsync(DosageForm dosageForm)
        {
            var validationResult = await dosageFormValidator.ValidateAsync(dosageForm);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingDosageForm = await dosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name);
            if (existingDosageForm != null)
            {
                throw new DomainException("Dosage form already exists");
            }
            return await dosageFormRepository.AddAsync(dosageForm);
        }

        public async Task<bool> DeleteDosageFormAsync(int id)
        {
            var dosageForm = await dosageFormRepository.GetDosageFormByIdAsync(id) ?? throw new DomainException("Dosage form not found");
            return await dosageFormRepository.DeleteAsync(dosageForm);          
        }

        public async Task<DosageForm?> GetDosageFormAsync(int id)
        {
            return await dosageFormRepository.GetDosageFormByIdAsync(id);
        }

        public async Task<List<DosageForm>> GetDosageFormsAsync()
        {
            return await dosageFormRepository.GetAllAsync();
        }

        public async Task<DosageForm> UpdateDosageFormAsync(DosageForm dosageForm)
        {
            var validationResult = await dosageFormValidator.ValidateAsync(dosageForm);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingDosageForm = await dosageFormRepository.GetDosageFormByIdAsync(dosageForm.Id) ?? throw new DomainException("Dosage form not found");
            var existingDosageFormWithSameName = await dosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name);
            if (existingDosageFormWithSameName != null && existingDosageFormWithSameName.Id != dosageForm.Id)
            {
                throw new DomainException("Dosage form with the same name already exists");
            }
            existingDosageForm.Name = dosageForm.Name;
            return await dosageFormRepository.UpdateAsync(existingDosageForm);
        }
    }
}
