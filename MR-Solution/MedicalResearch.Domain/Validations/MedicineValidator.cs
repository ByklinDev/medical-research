using FluentValidation;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class MedicineValidator: AbstractValidator<Medicine>
    {
        public MedicineValidator() 
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(2, 100).WithMessage("Description must be between 2 and 100 characters.");
            RuleFor(m => m.DosageFormId)
                .NotEmpty().WithMessage("Dosage Form ID is required.");
            RuleFor(m => m.MedicineContainerId)
                .NotEmpty().WithMessage("Medicine Container ID is required.");
            RuleFor(m => m.ExpireAt)
                .NotEmpty().WithMessage("Expiration Date is required.")
                .GreaterThan(DateTime.Now).WithMessage("Expiration Date must be in the future.");
            RuleFor(m => m.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(m => m.MedicineTypeId)
                .NotEmpty().WithMessage("Medicine Type ID is required.");
            RuleFor(m => m.CreatedAt)
                .NotEmpty().WithMessage("Creation Date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Creation Date must be in the past or present.");
            RuleFor(m => m.State)
                .NotEmpty().WithMessage("State is required.")
                .IsInEnum().WithMessage("State must be a valid enum value.");
        }
    }
}