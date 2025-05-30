using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository;

public interface ISupplyRepository: IBaseRepository<Supply>
{
    Task<List<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query);
    Task<List<Supply>> SearchByTermAsync(int? clinicId, int? medicineId, Query query);
}