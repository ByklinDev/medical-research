using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
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
    public class MedicineContainerService(IUnitOfWork unitOfWork, IValidator<MedicineContainer> medicineContainerValidator) : IMedicineContainerService
    {
        public async Task<MedicineContainer> AddMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            var validationResult = await medicineContainerValidator.ValidateAsync(medicineContainer);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicineContainer = await unitOfWork.MedicineContainerRepository.GetMedicineContainerByNameAsync(medicineContainer.Name);
            if (existingMedicineContainer != null)
            {
                throw new DomainException("Medicine container already exists.");
            }
            var added =  await unitOfWork.MedicineContainerRepository.AddAsync(medicineContainer);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Medicine container not added.");
        }

        public async Task<bool> DeleteMedicineContainerAsync(int id)
        {
            var existingMedicineContainer = await unitOfWork.MedicineContainerRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine container not found.");
            var isDelete =  unitOfWork.MedicineContainerRepository.Delete(existingMedicineContainer);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }

        public async Task<MedicineContainer?> GetMedicineContainerAsync(int id)
        {
            return await unitOfWork.MedicineContainerRepository.GetByIdAsync(id);
        }

        public async Task<List<MedicineContainer>> GetMedicineContainersAsync()
        {
            return await unitOfWork.MedicineContainerRepository.GetAllAsync();
        }

        public async Task<MedicineContainer> UpdateMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            var validationResult = await medicineContainerValidator.ValidateAsync(medicineContainer);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingMedicineContainer = await unitOfWork.MedicineContainerRepository.GetByIdAsync(medicineContainer.Id) ?? throw new DomainException("Medicine container not found");
            var existingMedicineContainerByName = await unitOfWork.MedicineContainerRepository.GetMedicineContainerByNameAsync(medicineContainer.Name);
            if (existingMedicineContainerByName != null && existingMedicineContainerByName.Id != medicineContainer.Id)
            {
                throw new DomainException("Medicine container with this name already exists");
            }
            existingMedicineContainer.Name = medicineContainer.Name;
            var updated = unitOfWork.MedicineContainerRepository.Update(existingMedicineContainer);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Medicine container not updated");
        }
    }
}
