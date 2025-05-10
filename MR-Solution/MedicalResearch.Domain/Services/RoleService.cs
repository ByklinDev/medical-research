using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
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
    public class RoleService(IUnitOfWork unitOfWork, IValidator<Role> roleValidator) : IRoleService
    {
        public async Task<Role> AddRoleAsync(Role role)
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

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await unitOfWork.RoleRepository.GetByIdAsync(id) ?? throw new DomainException("Role not found");
            var isDelete = unitOfWork.RoleRepository.Delete(role);
            return isDelete && await unitOfWork.SaveAsync() > 0;
        }

        public async Task<Role?> GetRoleAsync(int id)
        {
            return await unitOfWork.RoleRepository.GetByIdAsync(id);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await unitOfWork.RoleRepository.GetAllAsync(); 
        }

        public async Task<Role> UpdateRoleAsync(Role role)
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
    }
}
