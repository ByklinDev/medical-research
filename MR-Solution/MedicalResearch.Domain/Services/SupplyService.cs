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
    public class SupplyService(IUnitOfWork unitOfWork) : ISupplyService
    {
        public async Task<List<Supply>> AddSupplyAsync(List<Supply> supplies)
        {
            List<Supply> addedSupplies = [];
            foreach (var supply in supplies)
            {
                var medicineToSupply = await unitOfWork.MedicineRepository.GetByIdAsync(supply.MedicineId) ?? throw new DomainException("Medicine not found");
                if (medicineToSupply.Amount < supply.Amount)
                {
                    throw new DomainException("Not enough medicine in stock");
                }
                else
                {
                    medicineToSupply.Amount -= supply.Amount;
                    unitOfWork.MedicineRepository.Update(medicineToSupply);
                }
                var addedSupply = await unitOfWork.SupplyRepository.AddAsync(supply);

                var clinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(supply.MedicineId, supply.ClinicId);
                if (clinicStockMedicine != null)
                {
                    clinicStockMedicine.Amount += supply.Amount;
                    unitOfWork.ClinicStockMedicineRepository.Update(clinicStockMedicine);
                }
                else
                {
                    var newClinicStockMedicine = new ClinicStockMedicine
                    {
                        MedicineId = supply.MedicineId,
                        ClinicId = supply.ClinicId,
                        Amount = supply.Amount
                    };
                    await unitOfWork.ClinicStockMedicineRepository.AddAsync(newClinicStockMedicine);
                }
                addedSupplies.Add(addedSupply);
            }
            await unitOfWork.SaveAsync();
            return addedSupplies;
        }

        public async Task<Supply> AddToSupply(Medicine medicine, int amount, int clinicId)
        {
            var medicineToSupply = await unitOfWork.MedicineRepository.GetByIdAsync(medicine.Id) ?? throw new DomainException("Medicine not found");
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
            var existingSupply = await unitOfWork.SupplyRepository.GetByIdAsync(id) ?? throw new DomainException("Supply not found");
            var medicine = await unitOfWork.MedicineRepository.GetByIdAsync(existingSupply.MedicineId) ?? throw new DomainException("Medicine not found");
            medicine.Amount += existingSupply.Amount;
            unitOfWork.MedicineRepository.Update(medicine);
            var clinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
            if (clinicStockMedicine != null)
            {
                clinicStockMedicine.Amount -= existingSupply.Amount;
                unitOfWork.ClinicStockMedicineRepository.Update(clinicStockMedicine);
            }
            else
            {
                throw new DomainException("Clinic stock medicine not found");
            }
            var isDelete = unitOfWork.SupplyRepository.Delete(existingSupply);
            return await unitOfWork.SaveAsync() > 0 ? isDelete : throw new DomainException("Supply not deleted");
        }

        public async Task<List<Supply>> GetSuppliesAsync()
        {
            return await unitOfWork.SupplyRepository.GetAllAsync();
        }

        public async Task<List<Supply>> GetSuppliesByClinicIdAsync(int clinicId)
        {
            return await unitOfWork.SupplyRepository.GetSuppliesByClinicIdAsync(clinicId);
        }       

        public async Task<List<Supply>> GetSuppliesByMedicineIdAsync(int medicineId)
        {
            return await unitOfWork.SupplyRepository.GetSuppliesByMedicineIdAsync(medicineId);
        }

        public async Task<Supply?> GetSupplyAsync(int id)
        {
            return await unitOfWork.SupplyRepository.GetByIdAsync(id);
        }

        public async Task<Supply> UpdateSupplyAsync(Supply supply)
        {
            var existingSupply = await unitOfWork.SupplyRepository.GetByIdAsync(supply.Id) ?? throw new DomainException("Supply not found");
            var diff = supply.Amount - existingSupply.Amount;
            var existingClinicStockMedicine = await unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(existingSupply.MedicineId, existingSupply.ClinicId);
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
            unitOfWork.ClinicStockMedicineRepository.Update(existingClinicStockMedicine);
            var updated = unitOfWork.SupplyRepository.Update(existingSupply);
            return await unitOfWork.SaveAsync() > 0 ? updated : throw new DomainException("Supply not updated");
        }
    }
}