using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class SupplyRepository(MedicalResearchDbContext _context) : BaseRepository<Supply>(_context), ISupplyRepository
{

    public async Task<List<Supply>> GetInactiveSuppliesByUserIdAsync(int userId, Query query)
    {
        var result = _dbSet.Where(x => x.UserId == userId && x.IsActive == false)
                           .Skip(query.Skip)
                           .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(Supply).GetProperty(query.SortColumn)?.Name ?? typeof(Supply).GetProperties().FirstOrDefault()?.Name;
        if (prop != null)
        {
            if (query.IsAscending)
            {
                result = result.OrderBy(t => prop);
            }
            else
            {
                result = result.OrderByDescending(t => prop);
            }
        }
        return await result.AsNoTracking().ToListAsync();
    }

    public async Task<List<Supply>> SearchByTermAsync(int? clinicId, int? medicineId, Query query)
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
        result = result.Skip(query.Skip)
                       .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(Supply).GetProperty(query.SortColumn)?.Name ?? typeof(Supply).GetProperties().FirstOrDefault()?.Name;
        if (prop != null)
        {
            if (query.IsAscending)
            {
                result = result.OrderBy(t => prop);
            }
            else
            {
                result = result.OrderByDescending(t => prop);
            }
        }
        return await result.AsNoTracking().ToListAsync();
    }
}