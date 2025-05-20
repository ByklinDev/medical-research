using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Service
{
    public interface IMedicineService
    {
        Task<Medicine> AddMedicineAsync(Medicine medicine);
        Task<bool> DeleteMedicineAsync(int id);
        Task<Medicine> UpdateMedicineAsync(Medicine medicine);
        Task<Medicine?> GetMedicineAsync(int id);
        Task<List<Medicine>> GetMedicinesAsync();
    }
}
