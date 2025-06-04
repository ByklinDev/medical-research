using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
namespace MedicalResearch.Domain.Interfaces.Service;

public interface ISupplyService
{
    Task<List<Supply>> AddSupplyAsync(List<Supply> supplies, int userId);
    Task<Supply> AddToSupply(Medicine medicine, int amount, int clinicId, int userId);
    Task<bool> DeleteSupplyAsync(int id);
    Task<Supply> UpdateSupplyAsync(Supply supply);
    Task<Supply?> GetSupplyAsync(int id);
    Task<PagedList<Supply>> GetSuppliesAsync(int? clinicId, int? medicineId, Query query);
    Task<PagedList<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query);
}
