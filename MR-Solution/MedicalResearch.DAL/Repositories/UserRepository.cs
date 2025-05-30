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

internal class UserRepository(MedicalResearchDbContext _context): BaseRepository<User>(_context), IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbSet.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == email);
    }
    public async Task<List<User>> SearchByTermAsync(Query query)
    {
        var result = _dbSet.SearchByTerm(query.SearchTerm)
                           .Skip(query.Skip)
                           .Take(query.Take);
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(User).GetProperty(query.SortColumn)?.Name ?? typeof(User).GetProperties().FirstOrDefault()?.Name;
        if (prop != null)
        {
            if (query.IsAscending)
            {
                result = result.OrderBy(t => prop);
            }
            else
            {
                result = result.OrderByDescending(t => prop);
            }
        }
        return await result.AsNoTracking().ToListAsync();           
    }
}
