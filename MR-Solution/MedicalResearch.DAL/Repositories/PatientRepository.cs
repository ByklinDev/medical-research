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
    internal class PatientRepository(MedicalResearchDbContext _context) : BaseRepository<Patient>(_context), IPatientRepository
    {
        public async Task<Patient?> GetPatientByNumber(string number)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Number == number);
        }
    }
}
