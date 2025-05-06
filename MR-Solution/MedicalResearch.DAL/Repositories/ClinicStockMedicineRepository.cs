using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
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
            return await _context.Set<ClinicStockMedicine>()
                .Include(x => x.Clinic)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId)
        {
            return await _context.Set<ClinicStockMedicine>()
                .Include(x => x.Clinic)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => (x.ClinicId == clinicId) && (x.MedicineId == medicineId));
        }  
    }
}
