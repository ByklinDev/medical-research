using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface ISupplyRepository: IBaseRepository<Supply>
    {        
        Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId, Query query);
        Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId, Query query);
        Task<List<Supply>> GetSuppliesByParamsAsync(int clinicId, int medicineId, Query query);
        Task<List<Supply>> SearchByTermAsync(Query query);
    }
}