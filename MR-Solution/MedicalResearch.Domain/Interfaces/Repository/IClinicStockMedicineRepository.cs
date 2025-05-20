using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IClinicStockMedicineRepository: IBaseRepository<ClinicStockMedicine>
    {
        Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId); 
    }
}
