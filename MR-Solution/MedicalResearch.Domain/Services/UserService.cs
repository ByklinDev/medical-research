using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
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
    public class UserService(IUnitOfWork unitOfWork, IValidator<User> userValidator) : IUserService
    {
        public async Task<User> AddUserAsync(User user)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = unitOfWork.UserRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new DomainException("User with this email already exists");
            }
            user.Roles.Add(new Role() { Id = 3, Name = "Researcher" });
            var addedUser = await unitOfWork.UserRepository.AddAsync(user);
            return await unitOfWork.SaveAsync() > 0 ? addedUser : throw new DomainException("User not added");            
        }

        public async Task<bool> AddUserRole(User user, Role role)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            var existingRole = existingUser.Roles.FirstOrDefault(x => x.Id == role.Id);
            if (existingRole != null)
            {
                throw new DomainException("User already has this role");
            } 
            existingUser.Roles.Add(role);            
            var updated = unitOfWork.UserRepository.Update(existingUser); 
            return updated != null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new DomainException("User not found");
            var isDelete =  unitOfWork.UserRepository.Delete(user);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteUserRole(User user, Role role)
        {
            var userToDelete = await  unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            var existingRole = userToDelete.Roles.FirstOrDefault(x => x.Id == role.Id) ?? throw new DomainException("User does not have this role");
            userToDelete.Roles.Remove(existingRole);
            var updated = unitOfWork.UserRepository.Update(userToDelete);
            return updated != null;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await unitOfWork.UserRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var result = userValidator.Validate(user);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors[0].ToString());
            }
            var existingUser = await unitOfWork.UserRepository.GetByIdAsync(user.Id) ?? throw new DomainException("User not found");
            
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password; 
            existingUser.ClinicId = user.ClinicId;
            existingUser.Initials = user.Initials;
            existingUser.State = user.State;
            existingUser.PaswordSalt = user.PaswordSalt;
            existingUser.Roles = user.Roles;

            var updated = unitOfWork.UserRepository.Update(existingUser);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("User not updated");
        }

        public async Task<UserState> SetState(User user, UserState state)
        {
            var existingUser = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            existingUser.State = state;
            var updated = unitOfWork.UserRepository.Update(existingUser);
            return updated.State;
        }
    }
}
