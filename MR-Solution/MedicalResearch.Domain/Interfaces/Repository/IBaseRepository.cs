using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : Entiny
    {
        Task<T> AddAsync(T entity);
        bool Delete(T entity);
        T Update(T entity);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");
    }
}