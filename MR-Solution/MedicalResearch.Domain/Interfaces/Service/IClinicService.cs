using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IClinicService
    {
        Task<Clinic> AddClinicAsync(Clinic clinic);
        Task<bool> DeleteClinicAsync(int id);
        Task<Clinic> UpdateClinicAsync(Clinic clinic);
        Task<Clinic?> GetClinicAsync(int id);
        Task<List<Clinic>> GetClinicsAsync(Query query);
    }
}
