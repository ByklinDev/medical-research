using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IMedicineTypeRepository: IBaseRepository<MedicineType>
    {
        Task<MedicineType?> GetMedicineTypeByNameAsync(string name);
        Task<List<MedicineType>> SearchByTermAsync(Query query);
    }
}
