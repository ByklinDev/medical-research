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
    internal class MedicineContainerRepository(MedicalResearchDbContext _context) : BaseRepository<MedicineContainer>(_context), IMedicineContainerRepository
    {
        public async Task<MedicineContainer?> GetMedicineContainerByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
        }
    } 
}
