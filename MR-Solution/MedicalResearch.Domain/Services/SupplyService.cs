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
    public class SupplyService(ISupplyRepository supplyRepository, IMedicineRepository medicineRepository, IClinicStockMedicineRepository clinicStockMedicineRepository) : ISupplyService
    {
        public async Task<List<Supply>> AddSupplyAsync(List<Supply> supplies)
        {
            List<Supply> addedSupplies = [];
            foreach (var supply in supplies)
            {
                var medicineToSupply = await medicineRepository.GetMedicineByIdAsync(supply.MedicineId) ?? throw new DomainException("Medicine not found");
                if (medicineToSupply.Amount < supply.Amount)
                {
                    throw new DomainException("Not enough medicine in stock");
                }
                var addedSupply = await supplyRepository.AddAsync(supply);

                if (addedSupply != null)
                {
                    addedSupplies.Add(addedSupply);
                }
                else
                {
                    throw new DomainException("Failed to add supply");

                }
                var clinicStockMedicine = await clinicStockMedicineRepository.GetClinicStockMedicineAsync(supply.MedicineId, supply.ClinicId);
                if (clinicStockMedicine != null)
                {
                    clinicStockMedicine.Amount += supply.Amount;
                    await clinicStockMedicineRepository.UpdateAsync(clinicStockMedicine);
                }
                else
                {
                    var newClinicStockMedicine = new ClinicStockMedicine
                    {
                        MedicineId = supply.MedicineId,
                        ClinicId = supply.ClinicId,
                        Amount = supply.Amount
                    };
                    await clinicStockMedicineRepository.AddAsync(newClinicStockMedicine);
                }
                
            }
            return addedSupplies;
        }

        public async Task<Supply> AddToSupply(Medicine medicine, int amount, int clinicId)
        {
            var medicineToSupply = await medicineRepository.GetMedicineByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");
            if (medicineToSupply.Amount < amount)
            {
                throw new DomainException("Not enough medicine in stock");
            }
            return new Supply
            {
                MedicineId = medicine.Id,
                Amount = amount,
                ClinicId = clinicId,
                DateArrival = DateTime.Now
            };
        }

        public async Task<bool> DeleteSupplyAsync(int id)
        {
            var existingSupply = await supplyRepository.GetSupplyByIdAsync(id) ?? throw new DomainException("Supply not found");
            var medicine = await medicineRepository.GetMedicineByIdAsync(existingSupply.MedicineId) ?? throw new DomainException("Medicine not found");
            medicine.Amount += existingSupply.Amount;
            await medicineRepository.UpdateAsync(medicine);
            var clinicStockMedicine = await clinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
            if (clinicStockMedicine != null)
            {
                clinicStockMedicine.Amount -= existingSupply.Amount;
                await clinicStockMedicineRepository.UpdateAsync(clinicStockMedicine);
            }
            else
            {
                throw new DomainException("Clinic stock medicine not found");
            }
            return await supplyRepository.DeleteAsync(existingSupply);
        }

        public async Task<List<Supply>> GetSuppliesAsync()
        {
            return await supplyRepository.GetAllAsync();
        }

        public async Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId)
        {
            return await supplyRepository.GetSuppliesByClinicIdAsync(clinicId);
        }       

        public async Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId)
        {
            return await supplyRepository.GetSuppliesByMedicineIdAsync(medicineId);
        }

        public async Task<Supply?> GetSupplyAsync(int id)
        {
            return await supplyRepository.GetSupplyByIdAsync(id);
        }

        public async Task<Supply> UpdateSupplyAsync(Supply supply)
        {
            var existingSupply = await supplyRepository.GetSupplyByIdAsync(supply.Id) ?? throw new DomainException("Supply not found");
            var diff = supply.Amount - existingSupply.Amount;
            var existingClinicStockMedicine = await clinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
            if (existingClinicStockMedicine == null)
            {
                throw new DomainException("Clinic stock medicine not found");
            }
            if (existingClinicStockMedicine.Amount + diff < 0)
            {
                throw new DomainException("Not enough medicine in stock");
            }
            existingClinicStockMedicine.Amount += diff;
            existingSupply.Amount = supply.Amount;
            await clinicStockMedicineRepository.UpdateAsync(existingClinicStockMedicine);
            return await supplyRepository.UpdateAsync(existingSupply);
        }
    }
}