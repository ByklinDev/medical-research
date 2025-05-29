using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Repository;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class RoleService(IUnitOfWork unitOfWork, IValidator<Role> roleValidator, ILogger<RoleService> logger) : IRoleService
    {
        public async Task<Role> AddRoleAsync(Role role)
        {
            try
            {
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
                var added = await unitOfWork.RoleRepository.AddAsync(role);
                return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Role not added");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Role {role} could not be added: {message}", role.Name, ex.Message);
                throw new DomainException($"Error while adding Role {role.Name}");
            }
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await unitOfWork.RoleRepository.GetByIdAsync(id) ?? throw new DomainException("Role not found");
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

        public async Task<List<Role>> GetRolesAsync(Query query)
        {
            try
            {
                return await unitOfWork.RoleRepository.GetAllAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Roles could not be retrieved: {message}", ex.Message);
                throw new DomainException("Error while retrieving roles");
            }
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            try
            {
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
                existingRole.Name = role.Name;
                var updated = unitOfWork.RoleRepository.Update(existingRole);
                return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Role not updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Role {role} could not be updated: {message}", role.Name, ex.Message);
                throw new DomainException($"Error while updating Role {role.Name}");
            }
        }

        public async Task<List<Role>> GetRolesByNameAsync(Query query)
        {
            try
            {
                return await unitOfWork.RoleRepository.SearchByTermAsync(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Roles by name could not be retrieved: {message}", ex.Message);
                throw new DomainException("Error while retrieving roles by name");
            }
        }
    }
}
