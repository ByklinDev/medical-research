using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MedicalResearch.DAL.Repositories;

internal class MedicineContainerRepository(MedicalResearchDbContext _context) : BaseRepository<MedicineContainer>(_context), IMedicineContainerRepository
{
    public async Task<MedicineContainer?> GetMedicineContainerByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<PagedList<MedicineContainer>> SearchByTermAsync(Query query)
    {
        return await _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query); 
    }
}
