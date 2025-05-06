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
    internal class RoleRepository(MedicalResearchDbContext _context) : BaseRepository<Role>(_context), IRoleRepository
    {
        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Set<Role>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _context.Set<Role>().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
