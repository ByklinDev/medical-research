using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace MedicalResearch.Domain.Services;

public class VisitService(IUnitOfWork unitOfWork, ILogger<VisitService> logger) : IVisitService
{
    public async Task<Visit> AddVisitAsync(Visit visit)
    {
        Visit? added;
        int countAdded;
        ClinicStockMedicine? clinicStockMedicineUpdated;

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
        try
        {
            medicine.Amount -= 1;
            clinicStockMedicineUpdated = unitOfWork.ClinicStockMedicineRepository.Update(medicine);

            var numberOfVisit = unitOfWork.VisitRepository.GetNumberOfNextVisit(visit.PatientId);
            visit.NumberOfVisit = numberOfVisit;
            added = await unitOfWork.VisitRepository.AddAsync(visit);
            countAdded = await unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Visit {visit} could not be added: {message}", visit.Id, ex.Message);
            throw new DomainException($"Error while adding Visit {visit.Id}");
        }
        return countAdded > 0 && added != null && clinicStockMedicineUpdated != null ? added : throw new DomainException("Visit not added");
    }

    public async Task<bool> DeleteVisitAsync(int id)
    {
        ClinicStockMedicine? clinicStockMedicineUpdated;
        int countDeleted = 0;
        bool isDelete = false;

        var visit = await unitOfWork.VisitRepository.GetByIdAsync(id) ?? throw new DomainException("Visit not found");
        var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
        try
        {
            medicine.Amount += 1;
            clinicStockMedicineUpdated = unitOfWork.ClinicStockMedicineRepository.Update(medicine);
            if (clinicStockMedicineUpdated != null)
            {
                isDelete = unitOfWork.VisitRepository.Delete(visit);
            }
            if (isDelete)
            {
                countDeleted = await unitOfWork.SaveAsync();
            }
            return countDeleted > 0 && isDelete == true &&  clinicStockMedicineUpdated != null ? isDelete : throw new DomainException("Visit not deleted");
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

    public async Task<PagedList<Visit>> GetVisitsAsync(Query query)
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
        Visit? updated;
        int countUpdated = 0;

        var existingVisit = await unitOfWork.VisitRepository.GetByIdAsync(visit.Id) ?? throw new DomainException("Visit not found");
        var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
        var existedVisits = await unitOfWork.VisitRepository.GetVisitsOfPatient(visit.PatientId);
        var existedNumber = existedVisits.FirstOrDefault(x => x.ClinicId.Equals(visit.ClinicId) && x.PatientId.Equals(visit.PatientId) && x.NumberOfVisit.Equals(visit.NumberOfVisit));
        if (existedNumber != null)
        {
            throw new DomainException("Number of visit already exists");
        }
        try
        {
            existingVisit.DateOfVisit = visit.DateOfVisit;
            existingVisit.ClinicId = visit.ClinicId;
            existingVisit.PatientId = visit.PatientId;
            existingVisit.MedicineId = visit.MedicineId;
            existingVisit.NumberOfVisit = visit.NumberOfVisit;
            updated = unitOfWork.VisitRepository.Update(existingVisit);
            if (updated != null)
            {
                countUpdated = await unitOfWork.SaveAsync();
            }
            return countUpdated > 0 && updated != null ? updated : throw new DomainException("Visit not updated");
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
