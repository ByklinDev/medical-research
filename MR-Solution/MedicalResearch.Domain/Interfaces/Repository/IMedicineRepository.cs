using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IMedicineRepository: IBaseRepository<Medicine>
    {
        Task<Medicine?> GetMedicineByDescriptionAsync(string description);
        Task<Medicine?> GetMedicineAsync(Medicine medicine);
        Task<List<Medicine>> SearchByTermAsync(Query query);
    }
}