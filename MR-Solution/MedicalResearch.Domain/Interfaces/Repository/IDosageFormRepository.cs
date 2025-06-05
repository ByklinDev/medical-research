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
    public interface IDosageFormRepository: IBaseRepository<DosageForm>
    {
        Task<DosageForm?> GetDosageFormByNameAsync(string name);
        Task<PagedList<DosageForm>> SearchByTermAsync(Query query);
    }
}
