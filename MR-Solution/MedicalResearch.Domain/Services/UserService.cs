using FluentValidation;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class UserService(IUserRepository userRepository, IValidator<User> userValidator) : IUserService
    {
        public async Task<User> AddUserAsync(User user)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new DomainException("User with this email already exists");
            }
            user.Roles.Add(new Role() { Id = 3, Name = "Researcher" });
            return await userRepository.AddAsync(user);
        }

        public async Task<bool> AddUserRole(User user, Role role)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = await userRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            var existingRole = existingUser.Roles.FirstOrDefault(x => x.Id == role.Id);
            if (existingRole != null)
            {
                throw new DomainException("User already has this role");
            } 
            existingUser.Roles.Add(role);            
            await userRepository.UpdateAsync(existingUser); 
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id) ?? throw new DomainException("User not found");
            var result = await userRepository.DeleteAsync(user);
            if (!result)
            {
                throw new DomainException("User not deleted");
            }
            return result;
        }

        public async Task<bool> DeleteUserRole(User user, Role role)
        {
            var userToDelete = await userRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            var existingRole = userToDelete.Roles.FirstOrDefault(x => x.Id == role.Id) ?? throw new DomainException("User does not have this role");
            userToDelete.Roles.Remove(existingRole);
            await userRepository.UpdateAsync(userToDelete);
            return true;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await userRepository.GetAllAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = await userRepository.GetUserByIdAsync(user.Id) ?? throw new DomainException("User not found");
            
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password; 
            existingUser.ClinicId = user.ClinicId;
            existingUser.Initials = user.Initials;
            existingUser.State = user.State;
            existingUser.PaswordSalt = user.PaswordSalt;
            existingUser.Roles = user.Roles;

            return await userRepository.UpdateAsync(existingUser);

        }

        public async Task<UserState> SetState(User user, UserState state)
        {
            var existingUser = await userRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            existingUser.State = state;
            await userRepository.UpdateAsync(existingUser);
            return state;
        }
    }
}
