using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class PatientService(IUnitOfWork unitOfWork, IValidator<Patient> patientValidator, ILogger<PatientService> logger): IPatientService
{
    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        try
        {
            var validationResult = await patientValidator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingPatient = await unitOfWork.PatientRepository.GetPatientByNumber(patient.Number);
            if (existingPatient != null)
            {
                throw new DomainException("Patient with this number already exists");
            }
            var added = await unitOfWork.PatientRepository.AddAsync(patient);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Patient not added");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Patient {patient} could not be added: {message}", patient.Number, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient {patient} could not be added: {message}", patient.Number, ex.Message);
            throw new DomainException($"Error while adding Patient {patient.Number}");
        }
    }
    public async Task<bool> DeletePatientAsync(int id)
    {
        try
        {
            var patient = await unitOfWork.PatientRepository.GetByIdAsync(id) ?? throw new DomainException("Patient not found");
            var isDelete = unitOfWork.PatientRepository.Delete(patient);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Patient with id {id} could not be deleted: {message}", id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Patient with id {id}");
        }
    }
    public async Task<Patient?> GetPatientAsync(int id)
    {
        try
        {
            return await unitOfWork.PatientRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Patient with id {id}");
        }
    }
    public async Task<List<Patient>> GetPatientsAsync(Query query)
    {
        try
        {
            return await unitOfWork.PatientRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patients could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving Patients");
        }
    }
    public async Task<Patient> UpdatePatientAsync(Patient patient)
    {
        try
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
        catch (DomainException ex)
        {
            logger.LogError(ex, "Patient {patient} could not be updated: {message}", patient.Number, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient {patient} could not be updated: {message}", patient.Number, ex.Message);
            throw new DomainException($"Error while updating Patient {patient.Number}");
        }
    }

    public async Task<Patient?> GetPatientByNumber(string number)
    {
        try
        {
            return await unitOfWork.PatientRepository.GetPatientByNumber(number);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient with number {number} could not be retrieved: {message}", number, ex.Message);
            throw new DomainException($"Error while retrieving Patient with number {number}");
        }
    }
}   
