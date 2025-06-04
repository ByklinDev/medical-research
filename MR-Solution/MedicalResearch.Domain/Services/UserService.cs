using Microsoft.Extensions.Logging;
using FluentValidation;

using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using MedicalResearch.Domain.Extensions;

namespace MedicalResearch.Domain.Services;

public class UserService(IUnitOfWork unitOfWork, IValidator<User> userValidator, ILogger<UserService> logger) : IUserService
{
    public async Task<User> AddUserAsync(User user)
    {
        User? added;
        int countAdded;

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

        try
        {
            user.Roles.Add(new Role() { Id = 3, Name = "Researcher" });
            added = await unitOfWork.UserRepository.AddAsync(user);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while adding User {user.Email}");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("User not added");
    }

    public async Task<bool> AddUserRole(User user, Role role)
    {
        User? updated;
        int countUpdated;

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

        try
        {
            existingUser.Roles.Add(role);
            updated = unitOfWork.UserRepository.Update(existingUser);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be added: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while adding User {user.Email}");
        }
        return countUpdated > 0 && updated != null ? true : throw new DomainException("User role not added");
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new DomainException("User not found");
        try
        {
            var isDelete = unitOfWork.UserRepository.Delete(user);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting User with id {id}");
        }
    }

    public async Task<bool> DeleteUserRole(User user, Role role)
    {
        var userToDelete = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
        var existingRole = userToDelete.Roles.FirstOrDefault(x => x.Id == role.Id) ?? throw new DomainException("User does not have this role");

        try
        {
            userToDelete.Roles.Remove(existingRole);
            var updated = unitOfWork.UserRepository.Update(userToDelete);
            return updated != null && await unitOfWork.SaveAsync() > 0; 
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

    public async Task<PagedList<User>> GetUsersAsync(Query query)
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
        User? updated;
        int countUpdated;

        var result = userValidator.Validate(user);
        if (!result.IsValid)
        {
            throw new DomainException(result.Errors[0].ToString());
        }
        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(user.Id) ?? throw new DomainException("User not found");

        try
        {
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;
            existingUser.ClinicId = user.ClinicId;
            existingUser.Initials = user.Initials;
            existingUser.State = user.State;
            existingUser.PaswordSalt = user.PaswordSalt;
            existingUser.Roles = user.Roles;

            updated = unitOfWork.UserRepository.Update(existingUser);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} could not be updated: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while updating User {user.Email}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("User not updated");
    }

    public async Task<UserState> SetState(User user, UserState state)
    {
        User? updated;
        int countUpdated;

        var existingUser = await unitOfWork.UserRepository.GetUserByEmailAsync(user.Email) ?? throw new DomainException("User not found");
        try
        {
            existingUser.State = state;
            updated = unitOfWork.UserRepository.Update(existingUser);
            countUpdated = await unitOfWork.SaveAsync();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User {user} state could not be set: {message}", user.Email, ex.Message);
            throw new DomainException($"Error while setting state for User {user.Email}");
        }
        return countUpdated > 0 && updated != null ? updated.State : throw new DomainException("User state not set");
    }
}
