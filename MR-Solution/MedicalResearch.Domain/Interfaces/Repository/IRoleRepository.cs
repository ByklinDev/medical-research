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
    public interface IRoleRepository: IBaseRepository<Role>
    {
        Task<Role?> GetRoleByNameAsync(string name);
        Task<PagedList<Role>> SearchByTermAsync(Query query);
    }
}