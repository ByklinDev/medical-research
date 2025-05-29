using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using MedicalResearch.Domain.Validations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class MedicineContainerService(IUnitOfWork unitOfWork, IValidator<MedicineContainer> medicineContainerValidator, ILogger<MedicineContainerService> logger) : IMedicineContainerService
    {
        public async Task<MedicineContainer> AddMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            try
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
                var added = await unitOfWork.MedicineContainerRepository.AddAsync(medicineContainer);
                return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Medicine container not added.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine container {medicineContainer} could not be added: {message}", medicineContainer.Name, ex.Message);
                throw new DomainException($"Error while adding Medicine container {medicineContainer.Name}");
            }

        }

        public async Task<bool> DeleteMedicineContainerAsync(int id)
        {
            try
            {
                var existingMedicineContainer = await unitOfWork.MedicineContainerRepository.GetByIdAsync(id) ?? throw new DomainException("Medicine container not found.");
                var isDelete = unitOfWork.MedicineContainerRepository.Delete(existingMedicineContainer);
                return isDelete && await unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine container with id {id} could not be deleted: {message}", id, ex.Message);
                throw new DomainException($"Error while deleting Medicine container with id {id}");
            }
        }

        public async Task<MedicineContainer?> GetMedicineContainerAsync(int id)
        {
            try
            {
                return await unitOfWork.MedicineContainerRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine container with id {id} could not be retrieved: {message}", id, ex.Message);
                throw new DomainException($"Error while retrieving Medicine container with id {id}");
            }
        }

        public async Task<List<MedicineContainer>> GetMedicineContainersAsync(Query query)
        {
            try
            {
                return await unitOfWork.MedicineContainerRepository.GetAllAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while retrieving medicine containers: {message}", ex.Message);
                throw new DomainException("Error while retrieving medicine containers");
            }
        }

        public async Task<MedicineContainer> UpdateMedicineContainerAsync(MedicineContainer medicineContainer)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Medicine container {medicineContainer} could not be updated: {message}", medicineContainer.Name, ex.Message);
                throw new DomainException($"Error while updating Medicine container {medicineContainer.Name}");
            }
        }

        public async Task<List<MedicineContainer>> GetMedicineContainersByNameAsync(Query query)
        {
            try
            {
                return await unitOfWork.MedicineContainerRepository.SearchByTermAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while searching medicine containers by name: {message}", ex.Message);
                throw new DomainException("Error while searching medicine containers by name");
            }
        }
    }
}
