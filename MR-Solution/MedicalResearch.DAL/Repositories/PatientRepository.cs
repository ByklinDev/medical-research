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

    public async Task<PagedList<Patient>> SearchByTermAsync(Query query)
    {
        return await _dbSet.Include(x => x.Visits).ThenInclude(s => s.Medicine).SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        return await _dbSet.Include(x => x.Visits).FirstOrDefaultAsync(x => x.Id == id);
    }
}
