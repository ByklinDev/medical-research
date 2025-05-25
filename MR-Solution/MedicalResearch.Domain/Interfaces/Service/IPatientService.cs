using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IPatientService
    {
        Task<Patient> AddPatientAsync(Patient patient);
        Task<bool> DeletePatientAsync(int id);
        Task<Patient> UpdatePatientAsync(Patient patient);
        Task<Patient?> GetPatientAsync(int id);
        Task<List<Patient>> GetPatientsAsync();
        Patient? GetPatientByNumber(string number);
    }
}