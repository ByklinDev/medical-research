using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using Query = MedicalResearch.Domain.Queries.Query;

namespace MedicalResearch.Domain.Services;

public class PatientService(IUnitOfWork unitOfWork, IValidator<Patient> patientValidator, ILogger<PatientService> logger): IPatientService
{
    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        Patient? added;
        int countAdded;
        patient.DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth, DateTimeKind.Utc);
        var clinicId = Convert.ToInt32(patient.Number.Split('-')[0]);
        var clinic = await unitOfWork.ClinicRepository.GetByIdAsync(clinicId);
        if (clinic != null)
        {
            patient.Clinic = clinic;
        }
        patient.ClinicId = clinicId;
        patient.Status = PatientStatus.Screened;
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
    public async Task<PatientResearchDTO> UpdatePatientAsync(Patient patient)
    {
        Patient? updated;
        int countUpdated;
        PatientResearchDTO patientResearchDTO = new();
        var existingPatient = await unitOfWork.PatientRepository.GetPatientByIdAsync(patient.Id) ?? throw new DomainException("Patient not found");
        if (Enum.IsDefined(typeof(PatientStatus), patient.Status))
        {
            existingPatient.Status = patient.Status;
        }
        if (Enum.IsDefined(typeof(Sex), patient.Sex))
        {
            existingPatient.Sex = patient.Sex;
        }
        if (patient.DateOfBirth != new DateTime() && patient.DateOfBirth.AddYears(18) <= DateTime.UtcNow )
        {
            existingPatient.DateOfBirth = patient.DateOfBirth;
        }
       
        try
        {
            updated = unitOfWork.PatientRepository.Update(existingPatient);
            countUpdated = await unitOfWork.SaveAsync();

            var lastVisitDate = DateTime.MinValue;
            if (existingPatient.Visits.Count > 0)
            {
                lastVisitDate = existingPatient.Visits.Max(x => x.DateOfVisit);
            }
            patientResearchDTO = CreatePatientResearchDTO(existingPatient, lastVisitDate);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patient {patient} could not be updated: {message}", existingPatient.Number, ex.Message);
            throw new DomainException($"Error while updating Patient {existingPatient.Number}");
        }
        return countUpdated > 0 && patientResearchDTO != null ? patientResearchDTO : throw new DomainException("Patient not updated");
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

    public async Task<PatientResearchDTO?> GetPatientInfo(int id)
    {
        var patient = await unitOfWork.PatientRepository.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return null;
        }
        var lastVisitDate = DateTime.MinValue;
        if (patient.Visits.Count > 0)
        {
            lastVisitDate = patient.Visits.Max(x => x.DateOfVisit);
        }    
        return CreatePatientResearchDTO(patient, lastVisitDate);
    }

    public async Task<PagedList<PatientResearchDTO>> GetPatientsInfo(Query query)
    {
        List<PatientResearchDTO> result = new List<PatientResearchDTO>();
        var patients = await unitOfWork.PatientRepository.SearchByTermAsync(query);
        foreach (var patient in patients) 
        {
            var lastVisitDate = DateTime.MinValue;
            if (patient.Visits.Count > 0) 
            {
                lastVisitDate = patient.Visits.Max(x => x.DateOfVisit);   
            }            
            result.Add(CreatePatientResearchDTO(patient, lastVisitDate));        
        }

        var paged = new PagedList<PatientResearchDTO>(result, patients.TotalCount, patients.CurrentPage, patients.PageSize);
        return paged;
    }

    private static string GetAllPatientMedicines(List<Visit> visits)
    {
        var medicines = visits.Select(x => new { x.MedicineId, x.Medicine.Description }).Distinct().ToList();
        StringBuilder str = new StringBuilder();
        int i = 0;
        foreach (var medicine in medicines)
        {
            i++;
            str.Append(medicine.MedicineId.ToString());
            str.Append(". ");
            str.Append(medicine.Description);
            if (i < medicines.Count)
            {
                str.Append(", ");
            }
        }
        return str.ToString();
    }

    private static PatientResearchDTO CreatePatientResearchDTO(Patient patient, DateTime lastVisitDate)
    {
        return new PatientResearchDTO
        {
            Id = patient.Id,
            DateOfBirth = patient.DateOfBirth,
            Number = patient.Number,
            Medicines = GetAllPatientMedicines(patient.Visits),
            LastVisitDate = lastVisitDate,
            ClinicId = patient.ClinicId,
            Status = patient.Status,
            Sex = patient.Sex,
        };
    } 
}   
