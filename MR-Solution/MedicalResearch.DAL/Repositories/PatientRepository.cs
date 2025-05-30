using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class PatientRepository(MedicalResearchDbContext _context) : BaseRepository<Patient>(_context), IPatientRepository
{
    public async Task<Patient?> GetPatientByNumber(string number)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Number == number);
    }

    public async Task<List<Patient>> SearchByTermAsync(Query query)
    {
        var result = _dbSet.SearchByTerm(query.SearchTerm)
                           .Skip(query.Skip)
                           .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(Patient).GetProperty(query.SortColumn)?.Name ?? typeof(Patient).GetProperties().FirstOrDefault()?.Name;
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
