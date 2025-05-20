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
    public class ClinicService(IUnitOfWork unitOfWork, IValidator<Clinic> clinicValidator): IClinicService
    {
        public async Task<Clinic> AddClinicAsync(Clinic clinic)
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await unitOfWork.ClinicRepository.GetClinicByNameAsync(clinic.Name);
            if (existingClinic != null)
            {
                throw new DomainException("Clinic with the same name already exists");
            }
            var addedClinic = await unitOfWork.ClinicRepository.AddAsync(clinic);
            return await unitOfWork.SaveAsync() > 0 ? addedClinic : throw new DomainException("Clinic not added");
        }
        public async Task<bool> DeleteClinicAsync(int id)
        {
            var clinic = await unitOfWork.ClinicRepository.GetByIdAsync(id) ?? throw new DomainException("Clinic not found");
            var result = unitOfWork.ClinicRepository.Delete(clinic);
            return result && await unitOfWork.SaveAsync() > 0;
        }
        public async Task<Clinic?> GetClinicAsync(int id)
        {
            return await unitOfWork.ClinicRepository.GetByIdAsync(id);
        }
        public async Task<List<Clinic>> GetClinicsAsync()
        {
            return await unitOfWork.ClinicRepository.GetAllAsync();
        }
        public async Task<Clinic> UpdateClinicAsync(Clinic clinic)
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await unitOfWork.ClinicRepository.GetByIdAsync(clinic.Id) ?? throw new DomainException("Clinic not found");
            if (existingClinic.Name != clinic.Name)
            {
                var existingClinicWithSameName = await unitOfWork.ClinicRepository.GetClinicByNameAsync(clinic.Name);
                if (existingClinicWithSameName != null)
                {
                    throw new DomainException("Clinic with the same name already exists");
                }
            }
            existingClinic.Name = clinic.Name;
            existingClinic.Phone = clinic.Phone;
            existingClinic.City = clinic.City;
            existingClinic.AddressOne = clinic.AddressOne;
            existingClinic.AddressTwo = clinic.AddressTwo;
            var updatedClinic = unitOfWork.ClinicRepository.Update(existingClinic);
            return await unitOfWork.SaveAsync() > 0 ? updatedClinic : throw new DomainException("Clinic not updated");
        }
    }
}
