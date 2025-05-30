using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace MedicalResearch.Domain.Services;

public class ClinicService(IUnitOfWork unitOfWork, IValidator<Clinic> clinicValidator, ILogger<ClinicService> logger) : IClinicService
{
    public async Task<Clinic> AddClinicAsync(Clinic clinic)
    {
        try
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await unitOfWork.ClinicRepository.GetClinicByNameAsync(clinic.Name);
            if (existingClinic != null)
            {
                throw new DomainException("Clinic with the same name already exists");
            }
            var addedClinic = await unitOfWork.ClinicRepository.AddAsync(clinic);
            return await unitOfWork.SaveAsync() > 0 ? addedClinic : throw new DomainException("Clinic not added");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Clinic {clinic} could not be added: {message}", clinic.Name, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Clinic {clinic} could not be added: {message}", clinic.Name, ex.Message);
            throw new DomainException($"Error while adding Clinic {clinic.Name}");
        } 
    }
    public async Task<bool> DeleteClinicAsync(int id)
    {
        try
        {
            var clinic = await unitOfWork.ClinicRepository.GetByIdAsync(id) ?? throw new DomainException("Clinic not found");
            var result = unitOfWork.ClinicRepository.Delete(clinic);
            return result && await unitOfWork.SaveAsync() > 0;
        
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Clinic with id {id} could not be deleted: {message}", id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Clinic with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Clinic with id {id}");
        }
    }
    public async Task<Clinic?> GetClinicAsync(int id)
    {
        try
        {
            return await unitOfWork.ClinicRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Clinic with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Clinic with id {id}");
        }
    }
    public async Task<List<Clinic>> GetClinicsAsync(Query query)
    {
        try
        {
            return await unitOfWork.ClinicRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Clinics could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving Clinics");
        }
    }
    public async Task<Clinic> UpdateClinicAsync(Clinic clinic)
    {
        try
        {
            var result = await clinicValidator.ValidateAsync(clinic);
            if (!result.IsValid)
            {
                throw new DomainException(result.Errors.First().ErrorMessage);
            }
            var existingClinic = await unitOfWork.ClinicRepository.GetByIdAsync(clinic.Id) ?? throw new DomainException("Clinic not found");
            if (existingClinic.Name != clinic.Name)
            {
                var existingClinicWithSameName = await unitOfWork.ClinicRepository.GetClinicByNameAsync(clinic.Name);
                if (existingClinicWithSameName != null)
                {
                    throw new DomainException("Clinic with the same name already exists");
                }
            }
            existingClinic.Name = clinic.Name;
            existingClinic.Phone = clinic.Phone;
            existingClinic.City = clinic.City;
            existingClinic.AddressOne = clinic.AddressOne;
            existingClinic.AddressTwo = clinic.AddressTwo;
            var updatedClinic = unitOfWork.ClinicRepository.Update(existingClinic);
            return await unitOfWork.SaveAsync() > 0 ? updatedClinic : throw new DomainException("Clinic not updated");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Clinic {clinic} could not be updated: {message}", clinic.Name, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Clinic {clinic} could not be updated: {message}", clinic.Name, ex.Message);
            throw new DomainException($"Error while updating Clinic {clinic.Name}");
        }
    }      
}
