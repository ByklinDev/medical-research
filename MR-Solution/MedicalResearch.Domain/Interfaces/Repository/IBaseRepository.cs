using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);
        bool Delete(T entity);
        T Update(T entity);
        Task<PagedList<T>> GetAllAsync(Query query);
        Task<T?> GetByIdAsync(int id);
       
    }
}