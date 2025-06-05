using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories;

internal class RoleRepository(MedicalResearchDbContext _context) : BaseRepository<Role>(_context), IRoleRepository
{
    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<PagedList<Role>> SearchByTermAsync(Query query)
    {
        return await _dbSet.SearchByTerm(query.SearchTerm).SortSkipTakeAsync(query);
    }
}
