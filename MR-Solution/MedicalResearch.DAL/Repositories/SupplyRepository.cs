using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
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
        public async Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId)
        {
            return await _dbSet.Where(x => x.ClinicId == clinicId).ToListAsync();
        }
        public async Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId)
        {
            return await _dbSet.Where(x => x.MedicineId == medicineId).ToListAsync();
        }

        public async Task<List<Supply>> GetSuppliesByParamsAsync(int clinicId, int medicineId)
        {
            return await _dbSet.Where(x => x.MedicineId == medicineId && x.ClinicId == clinicId).ToListAsync();
        }
    }
}