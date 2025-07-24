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

internal class MedicineTypeRepository(MedicalResearchDbContext _context) : BaseRepository<MedicineType>(_context), IMedicineTypeRepository
{
    public async Task<MedicineType?> GetMedicineTypeByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<MedicineType> GetRandomMedicineTypeAsync()
    {
        Random rand = new Random();
        int toSkip = rand.Next(0,await _dbSet.CountAsync());
        var medicineType = await _dbSet.OrderBy(x => Guid.NewGuid()).Skip(toSkip).Take(1).FirstAsync();
        return medicineType;
    }

    public async Task<PagedList<MedicineType>> SearchByTermAsync(Query query)
    {
        return await _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }
}
