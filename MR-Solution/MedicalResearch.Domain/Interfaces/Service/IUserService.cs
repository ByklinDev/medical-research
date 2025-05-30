using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<User?> GetUserAsync(int id);
        Task<List<User>> GetUsersAsync(Query query);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> AddUserRole(User user, Role role);
        Task<bool> DeleteUserRole(User user, Role role);
        Task<UserState> SetState(User user, UserState state);
    }
}
