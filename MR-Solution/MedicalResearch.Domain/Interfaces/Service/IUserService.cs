using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using MedicalResearch.Domain.DTO;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User> UpdateUserAsync(UserUpdateDTO user);
        Task<User?> GetUserAsync(int id);
        Task<PagedList<User>> GetUsersAsync(Query query);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> AddUserRole(User user, Role role);
        Task<bool> DeleteUserRole(User user, Role role);
        Task<UserState> SetState(User user, UserState state);
        Task<string> SetImage(User user, byte[] image);
        Task<string> GetUserImage(int userid);
        Task<bool> ValidateUserAsync(string email, string password);
    }
}
