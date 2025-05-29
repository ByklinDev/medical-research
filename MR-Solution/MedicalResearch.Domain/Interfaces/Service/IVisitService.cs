using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IVisitService
    {
        Task<Visit> AddVisitAsync(Visit visit);
        Task<bool> DeleteVisitAsync(int id);
        Task<Visit> UpdateVisitAsync(Visit visit);
        Task<Visit?> GetVisitAsync(int id);
        Task<List<Visit>> GetVisitsAsync(Query query);
        int GetNumberOfNextVisit(int patientId);
        Task<List<Visit>> GetVisitsByNameAsync(Query query);
    }
}
