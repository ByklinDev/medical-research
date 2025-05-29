using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IMedicineTypeService
    {
        Task<MedicineType> AddMedicineTypeAsync(MedicineType medicineType);
        Task<bool> DeleteMedicineTypeAsync(int id);
        Task<MedicineType> UpdateMedicineTypeAsync(MedicineType medicineType);
        Task<MedicineType?> GetMedicineTypeAsync(int id);
        Task<List<MedicineType>> GetMedicineTypesAsync(Query query);
        Task<List<MedicineType>> GetMedicineTypesByNameAsync(Query query);
    }
}
