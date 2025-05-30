using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.DAL.Repositories;

internal class DosageFormRepository(MedicalResearchDbContext _context) : BaseRepository<DosageForm>(_context), IDosageFormRepository
{
    public async Task<DosageForm?> GetDosageFormByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<DosageForm>> SearchByTermAsync(Query query)
    {
        var result = _dbSet
            .SearchByTerm(query.SearchTerm)
            .Skip(query.Skip)
            .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(DosageForm).GetProperty(query.SortColumn)?.Name ?? typeof(DosageForm).GetProperties().FirstOrDefault()?.Name;
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