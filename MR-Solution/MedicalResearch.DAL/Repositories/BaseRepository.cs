using MedicalResearch.DAL.DataContext;
using MedicalResearch.Domain.Constants;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using MedicalResearch.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.Repositories;

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

    public virtual async Task<PagedList<T>> GetAllAsync(Query query)
    {
        var t = _dbSet.AsQueryable();
        if (string.IsNullOrEmpty(query.SortColumn))
        {
            query.SortColumn = "Id";
        }
        var prop = typeof(T).GetProperty(query.SortColumn);
        if (prop != null)
        {
            if (query.IsAscending)
            {
                t = t.OrderBy(t => EF.Property<object>(t, query.SortColumn));
            }
            else
            {
                t = t.OrderByDescending(t => EF.Property<object>(t, query.SortColumn));
            }
        }
        var result = await PagedList<T>.ToPagedList(t, query.Skip, query.Take);
        return result;
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