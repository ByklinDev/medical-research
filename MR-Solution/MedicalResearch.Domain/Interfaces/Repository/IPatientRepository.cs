using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IPatientRepository: IBaseRepository<Patient>
    {
        Task<Patient?> GetPatientByNumber(string number);
        Task<List<Patient>> SearchByTermAsync(Query query);
    }
}
