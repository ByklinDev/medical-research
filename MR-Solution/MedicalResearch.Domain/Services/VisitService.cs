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
    public class VisitService(IVisitRepository visitRepository) : IVisitService
    {
        public async Task<Visit> AddVisitAsync(Visit visit)
        {
            return await visitRepository.AddAsync(visit);
        }

        public async Task<bool> DeleteVisitAsync(int id)
        {
            var visit = await visitRepository.GetVisitByIdAsync(id) ?? throw new DomainException("Visit not found");
            return await visitRepository.DeleteAsync(visit);
        }

        public async Task<Visit?> GetVisitAsync(int id)
        {
            return await visitRepository.GetVisitByIdAsync(id);
        }

        public async Task<List<Visit>> GetVisitsAsync()
        {
            return await visitRepository.GetAllAsync();
        }

        public async Task<Visit> UpdateVisitAsync(Visit visit)
        {
            var existingVisit = await visitRepository.GetVisitByIdAsync(visit.Id) ?? throw new DomainException("Visit not found");
            existingVisit.DateOfVisit = visit.DateOfVisit;
            existingVisit.ClinicId = visit.ClinicId;
            existingVisit.PatientId = visit.PatientId;
            existingVisit.MedicineId = visit.MedicineId;
            existingVisit.NumberOfVisit = visit.NumberOfVisit;
            return await visitRepository.UpdateAsync(existingVisit);
        }
    }
}
