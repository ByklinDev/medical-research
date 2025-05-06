using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IMedicineContainerRepository: IBaseRepository<MedicineContainer>
    {
        Task<MedicineContainer?> GetMedicineContainerByIdAsync(int id);
        Task<MedicineContainer?> GetMedicineContainerByNameAsync(string name);
    }
}
