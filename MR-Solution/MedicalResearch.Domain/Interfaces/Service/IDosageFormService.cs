using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IDosageFormService
    {
        Task<DosageForm> AddDosageFormAsync(DosageForm dosageForm);
        Task<bool> DeleteDosageFormAsync(int id);
        Task<DosageForm> UpdateDosageFormAsync(DosageForm dosageForm);
        Task<DosageForm?> GetDosageFormAsync(int id);
        Task<List<DosageForm>> GetDosageFormsAsync();
    }
}
