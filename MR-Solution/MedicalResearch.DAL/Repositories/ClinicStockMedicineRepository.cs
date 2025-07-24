using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalResearch.DAL.Repositories;

internal class ClinicStockMedicineRepository(MedicalResearchDbContext _context) : BaseRepository<ClinicStockMedicine>(_context), IClinicStockMedicineRepository
{
    public override async Task<ClinicStockMedicine?> GetByIdAsync(int id)
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

    public async Task<ClinicStockMedicine?> GetRandomMedicineAsync(int clinicId, int medicineTypeId)
    {
        var medicines = await _dbSet.Where(x => x.Medicine.MedicineTypeId == medicineTypeId && x.ClinicId == clinicId && x.Amount > 0).ToListAsync();
        Random rand = new Random();
        int toSkip = rand.Next(0, medicines.Count);
        var randomMedicine = medicines.OrderBy(x=> Guid.NewGuid()).Skip(toSkip).Take(1).FirstOrDefault();
        return randomMedicine;
    }

    public async Task<PagedList<ClinicStockMedicine>> SearchByTermAsync(int? clinicId, Query query)
    {
        IQueryable<ClinicStockMedicine> result;
        if (clinicId != null && clinicId.HasValue && clinicId > 0)
        {
            result = _dbSet.SearchByTermAndClinic(query.SearchTerm, (int)clinicId);
        }
        else
        {
            result = _dbSet.SearchByTerm(query.SearchTerm);
        }
        return await result.SortSkipTakeAsync(query);
    }
}
