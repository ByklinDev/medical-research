using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories
{
    internal class SupplyRepository(MedicalResearchDbContext _context) : BaseRepository<Supply>(_context), ISupplyRepository
    {
        public async Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId, Query query)
        {
            return await _dbSet.Where(x => x.ClinicId == clinicId ).Skip(query.Skip).Take(query.Take).ToListAsync();
        }
        public async Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId, Query query)
        {
            return await _dbSet.Where(x => x.MedicineId == medicineId).Skip(query.Skip).Take(query.Take).ToListAsync();
        }

        public async Task<List<Supply>> GetSuppliesByParamsAsync(int clinicId, int medicineId, Query query)
        {
            return await _dbSet.Where(x => x.MedicineId == medicineId && x.ClinicId == clinicId).Skip(query.Skip).Take(query.Take).ToListAsync();
        }

        public async Task<List<Supply>> SearchByTermAsync(Query query)
        {
            return await _dbSet
                .SearchByTerm(query.SearchTerm)
                .Skip(query.Skip)
                .Take(query.Take > 0 ? query.Take : Int32.MaxValue)
                .OrderByDescending(t => t.DateArrival)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}