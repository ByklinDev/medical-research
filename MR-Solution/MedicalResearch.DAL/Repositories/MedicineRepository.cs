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
    internal class MedicineRepository(MedicalResearchDbContext _context): BaseRepository<Medicine>(_context), IMedicineRepository
    {
        public async Task<Medicine?> GetMedicineByIdAsync(int id)
        {
            return await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Medicine?> GetMedicineByDescriptionAsync(string description)
        {
            return await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Description == description);
        }
        public async Task<Medicine?> GetMedicineAsync(Medicine medicine)
        {
            return await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Description == medicine.Description 
                                        && x.MedicineContainerId == medicine.MedicineContainerId
                                        && x.MedicineTypeId == medicine.MedicineTypeId
                                        && x.DosageFormId == medicine.DosageFormId 
                                        && x.ExpireAt == medicine.ExpireAt);
        }
    }
}
