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
    public class VisitService(IUnitOfWork unitOfWork) : IVisitService
    {
        public async Task<Visit> AddVisitAsync(Visit visit)
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

        public async Task<bool> DeleteVisitAsync(int id)
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

        public async Task<Visit?> GetVisitAsync(int id)
        {
            return await unitOfWork.VisitRepository.GetByIdAsync(id);
        }

        public async Task<List<Visit>> GetVisitsAsync()
        {
            return await unitOfWork.VisitRepository.GetAllAsync();
        }

        public async Task<Visit> UpdateVisitAsync(Visit visit)
        {
            var existingVisit = await unitOfWork.VisitRepository.GetByIdAsync(visit.Id) ?? throw new DomainException("Visit not found");
            var medicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(visit.MedicineId, visit.ClinicId) ?? throw new DomainException("Medicine not found");
            existingVisit.DateOfVisit = visit.DateOfVisit;
            existingVisit.ClinicId = visit.ClinicId;
            existingVisit.PatientId = visit.PatientId;
            existingVisit.MedicineId = visit.MedicineId;
            var existedVisits = await unitOfWork.VisitRepository.GetVisitsOfPatient(visit.PatientId);
            var existedNumber =  existedVisits.FirstOrDefault(x => x.ClinicId.Equals(visit.ClinicId) && x.PatientId.Equals(visit.PatientId) && x.NumberOfVisit.Equals(visit.NumberOfVisit));
            if (existedNumber != null)
            {
                throw new DomainException("Number of visit already exists");
            }
            existingVisit.NumberOfVisit = visit.NumberOfVisit;
            var updated = unitOfWork.VisitRepository.Update(existingVisit);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Visit not updated");
        }

        public int GetNumberOfNextVisit(int patientId)
        {
            return unitOfWork.VisitRepository.GetNumberOfNextVisit(patientId);
        }
    }
}
