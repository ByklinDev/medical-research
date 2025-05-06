using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories
{
    internal class BaseRepository<T>(MedicalResearchDbContext _context): IBaseRepository<T> where T : class
    {        
        public virtual async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            var existingEntity = _context.Set<T>().Entry(entity).Entity;
            if (existingEntity == null)
            {
                return false;
            }
            _context.Set<T>().Remove(existingEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}