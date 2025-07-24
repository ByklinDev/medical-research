using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IClinicStockMedicineService
    {
        Task<ClinicStockMedicine?> GetClinicStockMedicineByIdAsync(int id);
        Task<ClinicStockMedicine?> GetClinicStockMedicineAsync(int clinicId, int medicineId);
        Task<PagedList<ClinicStockMedicine>> GetClinicStockMedicinesAsync(Query query);
        Task<PagedList<ClinicStockMedicine>> GetClinicStockMedicinesByClinicIdAsync(int clinicId, Query query);
        Task<ClinicStockMedicine?> GetRandomClinicStockMedicineAsync(int clinicId, int medicineTypeId);

    }
}