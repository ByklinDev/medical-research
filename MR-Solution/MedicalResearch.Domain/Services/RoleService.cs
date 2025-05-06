using FluentValidation;
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
    public class RoleService(IRoleRepository roleRepository, IValidator<Role> roleValidator) : IRoleService
    {
        public async Task<Role> AddRoleAsync(Role role)
        {
            var validationResult = await roleValidator.ValidateAsync(role);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingRole = await roleRepository.GetRoleByNameAsync(role.Name);
            if (existingRole != null)
            {
                throw new DomainException("Role already exists");
            }
            return await roleRepository.AddAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await roleRepository.GetRoleByIdAsync(id) ?? throw new DomainException("Role not found");
            return await roleRepository.DeleteAsync(role);
        }

        public async Task<Role?> GetRoleAsync(int id)
        {
            return await roleRepository.GetRoleByIdAsync(id);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await roleRepository.GetAllAsync(); 
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            var validationResult = await roleValidator.ValidateAsync(role);
            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors.First().ErrorMessage);
            }
            var existingRole = await roleRepository.GetRoleByIdAsync(role.Id) ?? throw new DomainException("Role not found");
            var roleWithSameName = await roleRepository.GetRoleByNameAsync(role.Name);
            if (roleWithSameName != null && roleWithSameName.Id != role.Id)
            {
                throw new DomainException("Role with the same name already exists");
            }
            existingRole.Name = role.Name;
            return await roleRepository.UpdateAsync(existingRole);
        }
    }
}
