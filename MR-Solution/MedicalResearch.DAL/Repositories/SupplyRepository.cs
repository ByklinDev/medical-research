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
        public async Task<Supply?> GetSupplyByIdAsync(int id)
        {
            return await _context.Set<Supply>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Supply> AddAsync(Supply entity)
        {
            var medicine = await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Id == entity.MedicineId) ?? throw new ArgumentException("Medicine not found");
            _context.Set<Supply>().Add(entity);
            medicine.Amount -= entity.Amount;
            _context.Set<Medicine>().Update(medicine);
            var clinicStockMedicine = await _context.Set<ClinicStockMedicine>().FirstOrDefaultAsync(x => x.MedicineId == entity.MedicineId && x.ClinicId == entity.ClinicId);
            if (clinicStockMedicine == null)
            {
                clinicStockMedicine = new ClinicStockMedicine
                {
                    MedicineId = entity.MedicineId,
                    ClinicId = entity.ClinicId,
                    Amount = entity.Amount
                };
                _context.Set<ClinicStockMedicine>().Add(clinicStockMedicine);
            }
            else
            {
                clinicStockMedicine.Amount += entity.Amount;
                _context.Set<ClinicStockMedicine>().Update(clinicStockMedicine);
            }
            await _context.SaveChangesAsync();
            return entity;
        }



        public override async Task<bool> DeleteAsync(Supply entity)
        {
            var existingEntity = _context.Set<Supply>().Entry(entity).Entity;
            if (existingEntity == null)
            {
                return false;
            }
            var medicine = await _context.Set<Medicine>().FirstOrDefaultAsync(x => x.Id == entity.MedicineId) ?? throw new ArgumentException("Medicine not found");
            medicine.Amount += entity.Amount;
            _context.Set<Medicine>().Update(medicine);
            var clinicStockMedicine = await _context.Set<ClinicStockMedicine>().FirstOrDefaultAsync(x => x.MedicineId == entity.MedicineId && x.ClinicId == entity.ClinicId);
            if (clinicStockMedicine != null)
            {
                clinicStockMedicine.Amount -= entity.Amount;
                if (clinicStockMedicine.Amount < 0)
                {
                    throw new ArgumentException("Clinic stock medicine amount cannot be negative");
                }
                else
                {
                    _context.Set<ClinicStockMedicine>().Update(clinicStockMedicine);
                }
            }
            _context.Set<Supply>().Remove(existingEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId)
        {
            return await _context.Set<Supply>().Where(x => x.ClinicId == clinicId).ToListAsync();
        }
        public async Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId)
        {
            return await _context.Set<Supply>().Where(x => x.MedicineId == medicineId).ToListAsync();
        }

        public async Task<List<Supply>> GetSuppliesByParamsAsync(int clinicId, int medicineId)
        {
            return await _context.Set<Supply>().Where(x => x.MedicineId == medicineId && x.ClinicId == clinicId).ToListAsync();
        }

    }
}