using Microsoft.Extensions.Logging;
using FluentValidation;

using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.Domain.Services;

public class UserService(IUnitOfWork unitOfWork, IValidator<User> userValidator, ILogger<UserService> logger) : IUserService
{
    public async Task<User> AddUserAsync(User user)
    {
        try
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
        catch (DomainException ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while adding User {user.Email}");
        }
    }

    public async Task<bool> AddUserRole(User user, Role role)
    {
        try
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
        catch (DomainException ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while adding User {user.Email}");
        }   
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new DomainException("User not found");
            var isDelete = unitOfWork.UserRepository.Delete(user);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "User with id {id} could not be deleted: {message}", id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting User with id {id}");
        }
    }

    public async Task<bool> DeleteUserRole(User user, Role role)
    {
        try
        {
            var userToDelete = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            var existingRole = userToDelete.Roles.FirstOrDefault(x => x.Id == role.Id) ?? throw new DomainException("User does not have this role");
            userToDelete.Roles.Remove(existingRole);
            var updated = unitOfWork.UserRepository.Update(userToDelete);
            return updated != null;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "User {user} role could not be deleted: {message}", user.Email, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be deleted: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while deleting User {user.Email}");
        }
    }

    public async Task<User?> GetUserAsync(int id)
    {
        try
        {
            return await unitOfWork.UserRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving User with id {id}");
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await unitOfWork.UserRepository.GetUserByEmailAsync(email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User with email {email} could not be retrieved: {message}", email, ex.Message);
            throw new DomainException($"Error while retrieving User with email {email}");
        }
    }

    public async Task<List<User>> GetUsersAsync(Query query)
    {
        try
        {
            return await unitOfWork.UserRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Users could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving Users");
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
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
        catch (DomainException ex)
        {
            logger.LogError(ex, "User {user} could not be updated: {message}", user.Email, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be updated: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while updating User {user.Email}");
        }
    }

    public async Task<UserState> SetState(User user, UserState state)
    {
        try
        {
            var existingUser = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
            existingUser.State = state;
            var updated = unitOfWork.UserRepository.Update(existingUser);
            return updated.State;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "User {user} state could not be set: {message}", user.Email, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} state could not be set: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while setting state for User {user.Email}");
        }
    }
}
