using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.DAL.Repositories
{
    internal class DosageFormRepository(MedicalResearchDbContext _context) : BaseRepository<DosageForm>(_context), IDosageFormRepository
    {
        public async Task<DosageForm?> GetDosageFormByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<DosageForm>> SearchByTermAsync(Query query)
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