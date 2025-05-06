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
    public class ClinicService(IClinicRepository clinicRepository, IValidator<Clinic> clinicValidator): IClinicService
    {
        public async Task<Clinic> AddClinicAsync(Clinic clinic)
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await clinicRepository.GetClinicByNameAsync(clinic.Name);
            if (existingClinic != null)
            {
                throw new DomainException("Clinic with the same name already exists");
            }
            return await clinicRepository.AddAsync(clinic);
        }
        public async Task<bool> DeleteClinicAsync(int id)
        {
            var clinic = await clinicRepository.GetClinicByIdAsync(id);
            if (clinic == null)
            {
                return false;
            }
            return await clinicRepository.DeleteAsync(clinic);
        }
        public async Task<Clinic?> GetClinicAsync(int id)
        {
            return await clinicRepository.GetClinicByIdAsync(id);
        }
        public async Task<List<Clinic>> GetClinicsAsync()
        {
            return await clinicRepository.GetAllAsync();
        }
        public async Task<Clinic> UpdateClinicAsync(Clinic clinic)
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await clinicRepository.GetClinicByIdAsync(clinic.Id) ?? throw new DomainException("Clinic not found");
            if (existingClinic.Name != clinic.Name)
            {
                var existingClinicWithSameName = await clinicRepository.GetClinicByNameAsync(clinic.Name);
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
            return await clinicRepository.UpdateAsync(existingClinic);
        }
    }
}
