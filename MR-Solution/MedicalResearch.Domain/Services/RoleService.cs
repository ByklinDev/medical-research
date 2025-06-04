using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class RoleService(IUnitOfWork unitOfWork, IValidator<Role> roleValidator, ILogger<RoleService> logger) : IRoleService
{
    public async Task<Role> AddRoleAsync(Role role)
    {
        Role? added;
        int countAdded;

        var validationResult = await roleValidator.ValidateAsync(role);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }
        var existingRole = await unitOfWork.RoleRepository.GetRoleByNameAsync(role.Name);
        if (existingRole != null)
        {
            throw new DomainException("Role already exists");
        }

        try
        {
            added = await unitOfWork.RoleRepository.AddAsync(role);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Role {role} could not be added: {message}", role.Name, ex.Message);
            throw new DomainException($"Error while adding Role {role.Name}");
        }
        return countAdded > 0 && added != null ? added : throw new DomainException("Role not added");
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await unitOfWork.RoleRepository.GetByIdAsync(id) ?? throw new DomainException("Role not found");
        try
        {
            var isDelete = unitOfWork.RoleRepository.Delete(role);
            return isDelete && await unitOfWork.SaveAsync() > 0;       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Role with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Role with id {id}");
        }
    }

    public async Task<Role?> GetRoleAsync(int id)
    {
        try
        {
            return await unitOfWork.RoleRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Role with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Role with id {id}");
        }
    }

    public async Task<PagedList<Role>> GetRolesAsync(Query query)
    {
        try
        {
            return await unitOfWork.RoleRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Roles could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving roles");
        }
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        Role? updated;
        int countUpdated;

        var validationResult = await roleValidator.ValidateAsync(role);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors.First().ErrorMessage);
        }
        var existingRole = await unitOfWork.RoleRepository.GetByIdAsync(role.Id) ?? throw new DomainException("Role not found");
        var roleWithSameName = await unitOfWork.RoleRepository.GetRoleByNameAsync(role.Name);
        if (roleWithSameName != null && roleWithSameName.Id != role.Id)
        {
            throw new DomainException("Role with the same name already exists");
        }

        try
        {
            existingRole.Name = role.Name;
            updated = unitOfWork.RoleRepository.Update(existingRole);
            countUpdated = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Role {role} could not be updated: {message}", role.Name, ex.Message);
            throw new DomainException($"Error while updating Role {role.Name}");
        }
        return countUpdated > 0 && updated != null ? updated : throw new DomainException("Role not updated");
    }
}
