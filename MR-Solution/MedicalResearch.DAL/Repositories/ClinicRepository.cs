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
    internal class ClinicRepository(MedicalResearchDbContext _context) : BaseRepository<Clinic>(_context), IClinicRepository
    {
        public async Task<Clinic?> GetClinicByIdAsync(int id)
        {
            return await _context.Set<Clinic>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Clinic?> GetClinicByNameAsync(string name)
        {
            return await _context.Set<Clinic>().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
