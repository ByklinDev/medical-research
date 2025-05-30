using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IClinicStockMedicineService
    {
        Task<ClinicStockMedicine?> GetClinicStockMedicineByIdAsync(int id);
        Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId);
        Task<List<ClinicStockMedicine>> GetClinicStockMedicinesAsync(Query query);
        Task<List<ClinicStockMedicine>> GetClinicStockMedicinesByClinicIdAsync(int clinicId, Query query);
    }
}