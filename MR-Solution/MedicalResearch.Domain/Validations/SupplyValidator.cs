using FluentValidation;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class SupplyValidator: AbstractValidator<Supply>
    {
        private readonly IClinicService _clinicService;
        private readonly IMedicineService _medicineService;
        public SupplyValidator(IClinicService clinicService, IMedicineService medicineService) 
        {
            _clinicService = clinicService;
            _medicineService = medicineService;

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0.");
            RuleFor(x => x.ClinicId)
                .GreaterThan(0)
                .WithMessage("Clinic ID must be greater than 0.")
                .MustAsync(async (clinicId, cancellationToken) =>
                {
                    return await _clinicService.GetClinicAsync(clinicId) != null;
                })
                .WithMessage("Clinic does not exist.");
            RuleFor(x => x.MedicineId)
                .GreaterThan(0)
                .WithMessage("Medicine ID must be greater than 0.")
                .MustAsync(async (medicineId, cancellationToken) =>
                {
                    return await _medicineService.GetMedicineAsync(medicineId) != null;
                })
                .WithMessage("Medicine does not exist.");
        }
    }
}
