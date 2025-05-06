using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IClinicRepository: IBaseRepository<Clinic>
    {
        Task<Clinic?> GetClinicByIdAsync(int id);
        Task<Clinic?> GetClinicByNameAsync(string name);
    }
}
