using MedicalResearch.Domain.Models;
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
        Task<List<Supply>> GetSuppliesAsync();
        Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId);
        Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId);
    }
}
