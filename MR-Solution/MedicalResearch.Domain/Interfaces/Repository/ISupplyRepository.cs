using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface ISupplyRepository: IBaseRepository<Supply>
    {
        Task<Supply?> GetSupplyByIdAsync(int id);         
        Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId);
        Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId);
    }
}