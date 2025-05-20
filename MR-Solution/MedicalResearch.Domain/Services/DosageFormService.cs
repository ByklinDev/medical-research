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
    public class DosageFormService(IUnitOfWork unitOfWork, IValidator<DosageForm> dosageFormValidator) : IDosageFormService
    {
        public async Task<DosageForm> AddDosageFormAsync(DosageForm dosageForm)
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

        public async Task<bool> DeleteDosageFormAsync(int id)
        {
            var dosageForm = await unitOfWork.DosageFormRepository.GetByIdAsync(id) ?? throw new DomainException("Dosage form not found");
            var result = unitOfWork.DosageFormRepository.Delete(dosageForm);
            return  result && await unitOfWork.SaveAsync() > 0;
        }

        public async Task<DosageForm?> GetDosageFormAsync(int id)
        {
            return await unitOfWork.DosageFormRepository.GetByIdAsync(id);
        }

        public async Task<List<DosageForm>> GetDosageFormsAsync()
        {
            return await unitOfWork.DosageFormRepository.GetAllAsync();
        }

        public async Task<DosageForm> UpdateDosageFormAsync(DosageForm dosageForm)
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
    }
}
