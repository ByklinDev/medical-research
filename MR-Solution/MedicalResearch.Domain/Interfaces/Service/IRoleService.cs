using MedicalResearch.Domain.Models;
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
        Task<List<Role>> GetRolesAsync();
    }
}
