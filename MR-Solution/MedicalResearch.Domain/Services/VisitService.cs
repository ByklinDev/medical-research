using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class VisitService(IUnitOfWork unitOfWork, ILogger<VisitService> logger) : IVisitService
{
    public async Task<Visit> AddVisitAsync(Visit visit)
    {
        try
        {
            var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
            var clinic = await unitOfWork.ClinicRepository.GetByIdAsync(visit.ClinicId) ?? throw new DomainException("Clinic not found");
            var existedVisits = await unitOfWork.VisitRepository.GetVisitsOfPatient(visit.PatientId);
            var existedVisit = existedVisits.FirstOrDefault(x => x.ClinicId.Equals(visit.ClinicId) && x.DateOfVisit.Equals(visit.DateOfVisit));
            if (existedVisit != null)
            {
                throw new DomainException("Visit already exists");
            }
            if (medicine.Amount <= 0)
            {
                throw new DomainException("Not enough medicine in stock");
            }
            else
            {
                medicine.Amount -= 1;
                unitOfWork.ClinicStockMedicineRepository.Update(medicine);
            }
            var numberOfVisit = unitOfWork.VisitRepository.GetNumberOfNextVisit(visit.PatientId);
            visit.NumberOfVisit = numberOfVisit;
            var added = await unitOfWork.VisitRepository.AddAsync(visit);
            return await unitOfWork.SaveAsync() > 0 ? added : throw new DomainException("Visit not added");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Visit {visit} could not be added: {message}", visit.Id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visit {visit} could not be added: {message}", visit.Id, ex.Message);
            throw new DomainException($"Error while adding Visit {visit.Id}");
        }
    }

    public async Task<bool> DeleteVisitAsync(int id)
    {
        try
        {
            var visit = await unitOfWork.VisitRepository.GetByIdAsync(id) ?? throw new DomainException("Visit not found");
            var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
            if (medicine != null)
            {
                medicine.Amount += 1;
                unitOfWork.ClinicStockMedicineRepository.Update(medicine);
            }
            var isDelete = unitOfWork.VisitRepository.Delete(visit);
            return await unitOfWork.SaveAsync() > 0 ? isDelete : throw new DomainException("Visit not deleted");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Visit with id {id} could not be deleted: {message}", id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visit with id {id} could not be deleted: {message}", id, ex.Message);
            throw new DomainException($"Error while deleting Visit with id {id}");
        }
    }

    public async Task<Visit?> GetVisitAsync(int id)
    {
        try
        {
            return await unitOfWork.VisitRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visit with id {id} could not be retrieved: {message}", id, ex.Message);
            throw new DomainException($"Error while retrieving Visit with id {id}");
        }
    }

    public async Task<List<Visit>> GetVisitsAsync(Query query)
    {
        try
        {
            return await unitOfWork.VisitRepository.SearchByTermAsync(query);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visits could not be retrieved: {message}", ex.Message);
            throw new DomainException("Error while retrieving Visits");
        }
    }

    public async Task<Visit> UpdateVisitAsync(Visit visit)
    {
        try
        {
            var existingVisit = await unitOfWork.VisitRepository.GetByIdAsync(visit.Id) ?? throw new DomainException("Visit not found");
            var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
            existingVisit.DateOfVisit = visit.DateOfVisit;
            existingVisit.ClinicId = visit.ClinicId;
            existingVisit.PatientId = visit.PatientId;
            existingVisit.MedicineId = visit.MedicineId;
            var existedVisits = await unitOfWork.VisitRepository.GetVisitsOfPatient(visit.PatientId);
            var existedNumber = existedVisits.FirstOrDefault(x => x.ClinicId.Equals(visit.ClinicId) && x.PatientId.Equals(visit.PatientId) && x.NumberOfVisit.Equals(visit.NumberOfVisit));
            if (existedNumber != null)
            {
                throw new DomainException("Number of visit already exists");
            }
            existingVisit.NumberOfVisit = visit.NumberOfVisit;
            var updated = unitOfWork.VisitRepository.Update(existingVisit);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Visit not updated");
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Visit {visit} could not be updated: {message}", visit.Id, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visit {visit} could not be updated: {message}", visit.Id, ex.Message);
            throw new DomainException($"Error while updating Visit {visit.Id}");
        }
    }

    public int GetNumberOfNextVisit(int patientId)
    {
        try
        {
            return unitOfWork.VisitRepository.GetNumberOfNextVisit(patientId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not get number of next visit for patient {patientId}: {message}", patientId, ex.Message);
            throw new DomainException($"Error while getting number of next visit for patient {patientId}");
        }
    }
}
