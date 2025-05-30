using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class VisitRepository(MedicalResearchDbContext _context) : BaseRepository<Visit>(_context), IVisitRepository
{
    public int GetNumberOfNextVisit(int patientId)
    {
        return _dbSet.Where(x => x.PatientId == patientId).Max(v => v.NumberOfVisit);
    } 
    
    public async Task<List<Visit>> GetVisitsOfPatient(int patientId)
    {
        return await _dbSet.Where(x => x.PatientId == patientId).ToListAsync();
    }

    public async Task<List<Visit>> SearchByTermAsync(Query query)
    {
        var result = _dbSet.SearchByTerm(query.SearchTerm)
                   .Skip(query.Skip)
                   .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(User).GetProperty(query.SortColumn)?.Name ?? typeof(User).GetProperties().FirstOrDefault()?.Name;
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