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
    public class PatientService(IUnitOfWork unitOfWork, IValidator<Patient> patientValidator): IPatientService
    {
        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            var validationResult = await patientValidator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingPatient = unitOfWork.PatientRepository.Get(x => x.ClinicId.Equals(patient.ClinicId) && x.Number.Equals(patient.Number)).FirstOrDefault();
            if (existingPatient != null) 
            {
                throw new DomainException("Patient with this number already exists");
            }
            var added = await unitOfWork.PatientRepository.AddAsync(patient);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Patient not added");
        }
        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await unitOfWork.PatientRepository.GetByIdAsync(id) ?? throw new DomainException("Patient not found");
            var isDelete = unitOfWork.PatientRepository.Delete(patient);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }
        public async Task<Patient?> GetPatientAsync(int id)
        {
            return await unitOfWork.PatientRepository.GetByIdAsync(id);
        }
        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await unitOfWork.PatientRepository.GetAllAsync();
        }
        public async Task<Patient> UpdatePatientAsync(Patient patient)
        {
            var validationResult = await patientValidator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingPatient = await unitOfWork.PatientRepository.GetByIdAsync(patient.Id) ?? throw new DomainException("Patient not found");
            existingPatient.Status = patient.Status;
            existingPatient.Sex = patient.Sex;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            var updated = unitOfWork.PatientRepository.Update(existingPatient);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Patient not updated");
        }
    }   
}
