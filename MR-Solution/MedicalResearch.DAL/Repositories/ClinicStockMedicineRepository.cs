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
    internal class ClinicStockMedicineRepository(MedicalResearchDbContext _context) : BaseRepository<ClinicStockMedicine>(_context), IClinicStockMedicineRepository
    {
        public async Task<ClinicStockMedicine?> GetClinicStockMedicineById(int id)
        {
            return await _dbSet
                .Include(x => x.Clinic)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId)
        {
            return await _dbSet
                .Include(x => x.Clinic)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => (x.ClinicId == clinicId) && (x.MedicineId == medicineId));
        }

        public async Task<List<ClinicStockMedicine>> SearchByTermAsync(int clinicId, Query query)
        {
            return await _dbSet
               .SearchByTermAndClinic(query.SearchTerm, clinicId)
               .Skip(query.Skip)
               .Take(query.Take > 0 ? query.Take : Int32.MaxValue)
               .OrderByDescending(t => t.Medicine.Description)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
