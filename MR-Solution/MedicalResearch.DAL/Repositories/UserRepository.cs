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
    internal class UserRepository(MedicalResearchDbContext _context): BaseRepository<User>(_context), IUserRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
