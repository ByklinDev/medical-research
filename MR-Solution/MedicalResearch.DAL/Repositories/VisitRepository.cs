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
        var visits = _dbSet.Where(x => x.PatientId == patientId).ToList();
        if (visits.Count > 0)
        {
            return visits.Max(x => x.NumberOfVisit) + 1;
        }
        return 1;
    } 
    
    public async Task<List<Visit>> GetVisitsOfPatient(int patientId)
    {
        return await _dbSet.Where(x => x.PatientId == patientId).ToListAsync();
    }

    public async Task<PagedList<Visit>> SearchByTermAsync(int patientId, Query query)
    {
        return await  _dbSet.Where(x => x.PatientId == patientId).Include(s => s.Medicine).ThenInclude(s=>s.MedicineType).Include(s => s.Clinic).SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }

    public async Task<PagedList<Visit>> SearchByTermAsync(Query query)
    {
        return await _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }
}