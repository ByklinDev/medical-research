using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
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
        Task<PagedList<ClinicStockMedicine>> SearchByTermAsync(int? clinicId, Query query);
    }
}
