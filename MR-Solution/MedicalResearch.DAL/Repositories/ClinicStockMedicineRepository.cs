using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class ClinicStockMedicineRepository(MedicalResearchDbContext _context) : BaseRepository<ClinicStockMedicine>(_context), IClinicStockMedicineRepository
{
    public override async Task<ClinicStockMedicine?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Clinic)
            .Include(x => x.Medicine)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId)
    {
        return await _dbSet
            .Include(x => x.Clinic)
            .Include(x => x.Medicine)
            .FirstOrDefaultAsync(x => (x.ClinicId == clinicId) && (x.MedicineId == medicineId));
    }

    public async Task<List<ClinicStockMedicine>> SearchByTermAsync(int? clinicId, Query query)
    {
        IQueryable<ClinicStockMedicine> result;
        if (clinicId != null && clinicId.HasValue && clinicId > 0)
        {
            result = _dbSet.SearchByTermAndClinic(query.SearchTerm, (int)clinicId);
        }
        else
        {
            result = _dbSet.SearchByTerm(query.SearchTerm);
        }

        result = result.Skip(query.Skip)
                       .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(ClinicStockMedicine).GetProperty(query.SortColumn)?.Name ?? typeof(ClinicStockMedicine).GetProperties().FirstOrDefault()?.Name;
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
