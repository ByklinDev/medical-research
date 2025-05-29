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

namespace MedicalResearch.DAL.Repositories
{
    internal class MedicineTypeRepository(MedicalResearchDbContext _context) : BaseRepository<MedicineType>(_context), IMedicineTypeRepository
    {
        public async Task<MedicineType?> GetMedicineTypeByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<MedicineType>> SearchByTermAsync(Query query)
        {
            return await _dbSet
                .SearchByTerm(query.SearchTerm)
                .Skip(query.Skip)
                .Take(query.Take > 0 ? query.Take : Int32.MaxValue)
                .OrderByDescending(t => t.Name)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
