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
    public interface IRoleService
    {
        Task<Role> AddRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int id);
        Task<Role> UpdateRoleAsync(Role role);
        Task<Role?> GetRoleAsync(int id);
        Task<PagedList<Role>> GetRolesAsync(Query query);
    }
}
