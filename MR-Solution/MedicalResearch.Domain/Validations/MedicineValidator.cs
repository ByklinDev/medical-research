using FluentValidation;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class MedicineValidator: AbstractValidator<Medicine>
    {
        private readonly IMedicineContainerService _medicineContainerService;
        private readonly IDosageFormService _dosageFormService;
        private readonly IMedicineTypeService _medicineTypeService;
        public MedicineValidator(IMedicineTypeService medicineTypeService, IMedicineContainerService medicineContainerService, IDosageFormService dosageFormService) 
        {
            _dosageFormService = dosageFormService;
            _medicineContainerService = medicineContainerService;
            _medicineTypeService = medicineTypeService;

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(2, 100).WithMessage("Description must be between 2 and 100 characters.");
            RuleFor(m => m.DosageFormId)
                .NotEmpty().WithMessage("Dosage Form ID is required.")
                .MustAsync(async (x, cancellationToken) => { return await _dosageFormService.GetDosageFormAsync(x) != null; })
                .WithMessage("Dosage Form does not exist.");
            RuleFor(m => m.MedicineContainerId)
                .NotEmpty().WithMessage("Medicine Container ID is required.")
                .MustAsync(async (x, cancellationToken) => { return await _medicineContainerService.GetMedicineContainerAsync(x) != null; })
                .WithMessage("Medicine Container does not exist.");
            RuleFor(m => m.ExpireAt)
                .NotEmpty().WithMessage("Expiration Date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiration Date must be in the future.");
            RuleFor(m => m.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(m => m.MedicineTypeId)
                .NotEmpty().WithMessage("Medicine Type ID is required.")
                .MustAsync(async (x, cancellationToken) => { return await _medicineTypeService.GetMedicineTypeAsync(x) != null; })
                .WithMessage("Medicine Type does not exist.");
            RuleFor(m => m.CreatedAt)
                .NotEmpty().WithMessage("Creation Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Creation Date must be in the past or present.");
            RuleFor(m => m.State)
                .NotEmpty().WithMessage("State is required.")
                .IsInEnum().WithMessage("State must be a valid enum value.");
        }
    }
}