using FluentValidation;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class MedicineContainerService(IMedicineContainerRepository medicineContainerRepository, IValidator<MedicineContainer> medicineContainerValidator) : IMedicineContainerService
    {
        public async Task<MedicineContainer> AddMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            var validationResult = await medicineContainerValidator.ValidateAsync(medicineContainer);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicineContainer = await medicineContainerRepository.GetMedicineContainerByNameAsync(medicineContainer.Name);
            if (existingMedicineContainer != null)
            {
                throw new DomainException("Medicine container already exists.");
            }
            return await medicineContainerRepository.AddAsync(medicineContainer);
        }

        public async Task<bool> DeleteMedicineContainerAsync(int id)
        {
            var existingMedicineContainer = await medicineContainerRepository.GetMedicineContainerByIdAsync(id) ?? throw new DomainException("Medicine container not found.");
            return await medicineContainerRepository.DeleteAsync(existingMedicineContainer);
        }

        public async Task<MedicineContainer?> GetMedicineContainerAsync(int id)
        {
            return await medicineContainerRepository.GetMedicineContainerByIdAsync(id);
        }

        public async Task<List<MedicineContainer>> GetMedicineContainersAsync()
        {
            return await medicineContainerRepository.GetAllAsync();
        }

        public async Task<MedicineContainer> UpdateMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            var validationResult = await medicineContainerValidator.ValidateAsync(medicineContainer);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicineContainer = await medicineContainerRepository.GetMedicineContainerByIdAsync(medicineContainer.Id) ?? throw new DomainException("Medicine container not found");
            var existingMedicineContainerByName = await medicineContainerRepository.GetMedicineContainerByNameAsync(medicineContainer.Name);
            if (existingMedicineContainerByName != null && existingMedicineContainerByName.Id != medicineContainer.Id)
            {
                throw new DomainException("Medicine container with this name already exists");
            }
            existingMedicineContainer.Name = medicineContainer.Name;
            return await medicineContainerRepository.UpdateAsync(existingMedicineContainer);
        }
    }
}
