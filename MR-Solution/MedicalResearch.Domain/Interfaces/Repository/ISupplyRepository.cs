using MedicalResearch.Domain.Extensions;
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
    Task<PagedList<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query);
    Task<PagedList<Supply>> SearchByTermAsync(int? clinicId, int? medicineId, Query query);
}