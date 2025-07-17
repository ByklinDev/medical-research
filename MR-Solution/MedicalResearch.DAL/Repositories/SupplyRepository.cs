using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class SupplyRepository(MedicalResearchDbContext _context) : BaseRepository<Supply>(_context), ISupplyRepository
{

    public async Task<PagedList<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query)
    {
        return await _dbSet.Where(x => x.UserId == userId && x.IsActive == false)
            .Include(s => s.Clinic )
            .Include(s => s.Medicine)
            .SortSkipTakeAsync(query);
    }


    public async Task<Supply?> GetSupplyByIdAsync(int id)
    {
        return await _dbSet.Where(x => x.Id == id).Include(s => s.Clinic).Include(s => s.Medicine).FirstOrDefaultAsync();
    }

    public async Task<PagedList<Supply>> SearchByTermAsync(int? clinicId, int? medicineId, Query query)
    {
        var result = _dbSet.SearchByTerm(query.SearchTerm);
        if (clinicId != null && clinicId.HasValue && clinicId > 0)
        {
            result = result.Where(x => x.ClinicId == clinicId);
        }
        if (medicineId != null && medicineId.HasValue && medicineId > 0)
        {
            result = result.Where(x => x.MedicineId == medicineId);
        }
        return await result.SortSkipTakeAsync(query);
    }
}