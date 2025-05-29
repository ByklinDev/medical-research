using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface ISupplyService
    {        
        Task<List<Supply>> AddSupplyAsync(List<Supply> supplies);
        Task<Supply> AddToSupply(Medicine medicine, int amount, int clinicId);
        Task<bool> DeleteSupplyAsync(int id);
        Task<Supply> UpdateSupplyAsync(Supply supply);
        Task<Supply?> GetSupplyAsync(int id);
        Task<List<Supply>> GetSuppliesAsync(Query query);
        Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId, Query query);
        Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId, Query query);
        Task<List<Supply>> GetSuppliesByParamsAsync(int clinicId, int medicineId, Query query);
        Task<List<Supply>> GetSuppliesByNameAsync(Query query);
    }
}
