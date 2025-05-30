using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories;

internal class MedicineRepository(MedicalResearchDbContext _context) : BaseRepository<Medicine>(_context), IMedicineRepository
{
    public async Task<Medicine?> GetMedicineByDescriptionAsync(string description)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Description.Contains(description));
    }

    public async Task<Medicine?> GetMedicineAsync(Medicine medicine)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Description == medicine.Description
        && x.DosageFormId == medicine.DosageFormId
        && x.MedicineContainerId == medicine.MedicineContainerId
        && x.MedicineTypeId == medicine.MedicineTypeId
        && x.ExpireAt == medicine.ExpireAt);
    }

    public async Task<List<Medicine>> SearchByTermAsync(Query query)
    {
        var result = _dbSet.SearchByTerm(query.SearchTerm)
                           .Skip(query.Skip)
                           .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(Medicine).GetProperty(query.SortColumn)?.Name ?? typeof(Medicine).GetProperties().FirstOrDefault()?.Name;
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
