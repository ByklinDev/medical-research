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
    internal class VisitRepository(MedicalResearchDbContext _context) : BaseRepository<Visit>(_context), IVisitRepository
    {
        public async Task<Visit?> GetVisitByIdAsync(int id)
        {
            return await _context.Set<Visit>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Visit> AddAsync(Visit entity)
        {
            var clinicStockMedicine = await _context.Set<ClinicStockMedicine>().FirstOrDefaultAsync( x => x.ClinicId == entity.ClinicId && x.MedicineId == entity.MedicineId) ?? throw new ArgumentException("Clinic stock medicine not found");
            if (clinicStockMedicine.Amount <= 0)
            {
                throw new ArgumentException("Not enough medicine in stock");
            }
            _context.Set<Visit>().Add(entity);
            clinicStockMedicine.Amount -= 1;
            _context.Set<ClinicStockMedicine>().Update(clinicStockMedicine);
            await _context.SaveChangesAsync();
            return entity; 
        }

        public override async Task<bool> DeleteAsync(Visit entity)
        {
            var existingEntity = _context.Set<Visit>().Entry(entity).Entity;
            if (existingEntity == null)
            {
                return false;
            }
            var clinicStockMedicine = await _context.Set<ClinicStockMedicine>().FirstOrDefaultAsync(x => x.ClinicId == entity.ClinicId && x.MedicineId == entity.MedicineId) ?? throw new ArgumentException("Clinic stock medicine not found");
            clinicStockMedicine.Amount += 1;
            _context.Set<ClinicStockMedicine>().Update(clinicStockMedicine);
            _context.Set<Visit>().Remove(existingEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}