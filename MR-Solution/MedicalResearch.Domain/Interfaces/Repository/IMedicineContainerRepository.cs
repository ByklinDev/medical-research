using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IMedicineContainerRepository: IBaseRepository<MedicineContainer>
    {
        Task<MedicineContainer?> GetMedicineContainerByNameAsync(string name);
        Task<List<MedicineContainer>> SearchByTermAsync(Query query);
    }
}
