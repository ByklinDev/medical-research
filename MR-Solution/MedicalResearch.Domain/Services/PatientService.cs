using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class PatientService(IUnitOfWork unitOfWork, IValidator<Patient> patientValidator, ILogger<PatientService> logger): IPatientService
{
    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        Patient? added;
        int countAdded;

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

        try
        {
            added = await unitOfWork.PatientRepository.AddAsync(patient);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient {patient} could not be added: {message}", patient.Number, ex.Message);
            throw new DomainException($"Error while adding Patient {patient.Number}");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("Patient not added");

    }
    public async Task<bool> DeletePatientAsync(int id)
    {
        var patient = await unitOfWork.PatientRepository.GetByIdAsync(id) ?? throw new DomainException("Patient not found");
        try
        {
            var isDelete = unitOfWork.PatientRepository.Delete(patient);
            return isDelete && await unitOfWork.SaveAsync() > 0;
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
    public async Task<PagedList<Patient>> GetPatientsAsync(Query query)
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
        Patient? updated;
        int countUpdated;

        var validationResult = await patientValidator.ValidateAsync(patient);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }
        var existingPatient = await unitOfWork.PatientRepository.GetByIdAsync(patient.Id) ?? throw new DomainException("Patient not found");

        try
        {
            existingPatient.Status = patient.Status;
            existingPatient.Sex = patient.Sex;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            updated = unitOfWork.PatientRepository.Update(existingPatient);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient {patient} could not be updated: {message}", patient.Number, ex.Message);
            throw new DomainException($"Error while updating Patient {patient.Number}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Patient not updated");
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
