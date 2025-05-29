using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly MedicalResearchDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(MedicalResearchDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual bool Delete(T entity)
        {
            var existingEntity = _dbSet.Entry(entity).Entity;
            if (existingEntity == null)
            {
                return false;
            }
            _dbSet.Remove(existingEntity);
            return true;
        }

        public virtual async Task<List<T>> GetAllAsync(Query query)
        {
            return await _dbSet.Skip(query.Skip).Take(query.Take > 0 ? query.Take : Int32.MaxValue).ToListAsync();
        }

        public virtual T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}