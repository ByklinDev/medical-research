using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IVisitRepository: IBaseRepository<Visit>
    {
        Task<Visit?> GetVisitByIdAsync(int id);
    }
}