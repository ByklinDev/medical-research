using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IDosageFormRepository: IBaseRepository<DosageForm>
    {
        Task<DosageForm?> GetDosageFormByIdAsync(int id);
        Task<DosageForm?> GetDosageFormByNameAsync(string name);
    }
}
