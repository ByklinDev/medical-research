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

    public async Task<PagedList<Visit>> SearchByTermAsync(Query query)
    {
        return await  _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }
}