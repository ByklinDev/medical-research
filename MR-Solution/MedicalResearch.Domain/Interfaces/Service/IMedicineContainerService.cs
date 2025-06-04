using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IMedicineContainerService
    {
        Task<MedicineContainer> AddMedicineContainerAsync(MedicineContainer medicineContainer);
        Task<bool> DeleteMedicineContainerAsync(int id);
        Task<MedicineContainer> UpdateMedicineContainerAsync(MedicineContainer medicineContainer);
        Task<MedicineContainer?> GetMedicineContainerAsync(int id);
        Task<PagedList<MedicineContainer>> GetMedicineContainersAsync(Query query);
    }
}
