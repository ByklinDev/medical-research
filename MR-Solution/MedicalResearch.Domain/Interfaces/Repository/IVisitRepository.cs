using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IVisitRepository: IBaseRepository<Visit>
    {
        int GetNumberOfNextVisit(int patientId);
        Task<List<Visit>> GetVisitsOfPatient(int patientId);
        Task<List<Visit>> SearchByTermAsync(Query query);
    }
}