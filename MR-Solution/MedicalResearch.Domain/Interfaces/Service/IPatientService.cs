using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.Domain.Interfaces.Service;

public interface IPatientService
{
    Task<Patient> AddPatientAsync(Patient patient);
    Task<bool> DeletePatientAsync(int id);
    Task<PatientResearchDTO> UpdatePatientAsync(Patient patient);
    Task<Patient?> GetPatientAsync(int id);
    Task<PagedList<Patient>> GetPatientsAsync(Query query);
    Task<Patient?> GetPatientByNumber(string number);
    Task<PatientResearchDTO?> GetPatientInfo(int id);
    Task<PagedList<PatientResearchDTO>> GetPatientsInfo(Query query);
}