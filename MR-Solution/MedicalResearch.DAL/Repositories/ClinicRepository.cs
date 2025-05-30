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

internal class ClinicRepository(MedicalResearchDbContext _context) : BaseRepository<Clinic>(_context), IClinicRepository
{
    public async Task<Clinic?> GetClinicByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Clinic>> SearchByTermAsync(Query query)
    {
        var result = _dbSet
           .SearchByTerm(query.SearchTerm)
           .Skip(query.Skip)
           .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(Clinic).GetProperty(query.SortColumn)?.Name ?? typeof(Clinic).GetProperties().FirstOrDefault()?.Name;
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
