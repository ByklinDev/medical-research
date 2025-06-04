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

    public async Task<PagedList<DosageForm>> SearchByTermAsync(Query query)
    {
        return await _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }
}