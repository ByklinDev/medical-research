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
    internal class VisitRepository(MedicalResearchDbContext _context) : BaseRepository<Visit>(_context), IVisitRepository
    {
        public int GetNumberOfNextVisit(int patientId)
        {
            return _dbSet.Where(x => x.PatientId == patientId).Max(v => v.NumberOfVisit);
        }   
    }
}