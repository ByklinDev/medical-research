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

namespace MedicalResearch.DAL.Repositories
{
    internal class VisitRepository(MedicalResearchDbContext _context) : BaseRepository<Visit>(_context), IVisitRepository
    {
        public int GetNumberOfNextVisit(int patientId)
        {
            return _dbSet.Where(x => x.PatientId == patientId).Max(v => v.NumberOfVisit);
        } 
        
        public async Task<List<Visit>> GetVisitsOfPatient(int patientId)
        {
            return await _dbSet.Where(x => x.PatientId == patientId).ToListAsync();
        }

        public async Task<List<Visit>> SearchByTermAsync(Query query)
        {
            return await _dbSet
                .SearchByTerm(query.SearchTerm)
                .Skip(query.Skip)
                .Take(query.Take > 0 ? query.Take : Int32.MaxValue)
                .OrderByDescending(t => t.DateOfVisit)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}