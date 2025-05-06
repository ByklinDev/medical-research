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
    public class PatientService(IPatientRepository patientRepository, IValidator<Patient> patientValidator): IPatientService
    {
        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            var validationResult = await patientValidator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            return await patientRepository.AddAsync(patient);
        }
        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await patientRepository.GetPatientByIdAsync(id) ?? throw new DomainException("Patient not found");
            return await patientRepository.DeleteAsync(patient);
        }
        public async Task<Patient?> GetPatientAsync(int id)
        {
            return await patientRepository.GetPatientByIdAsync(id);
        }
        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await patientRepository.GetAllAsync();
        }
        public async Task<Patient> UpdatePatientAsync(Patient patient)
        {
            var validationResult = await patientValidator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingPatient = await patientRepository.GetPatientByIdAsync(patient.Id) ?? throw new DomainException("Patient not found");
            existingPatient.Status = patient.Status;
            existingPatient.Sex = patient.Sex;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            return await patientRepository.UpdateAsync(existingPatient);            
        }
    }   
}
