using FluentValidation;
using MedicalResearch.Api.DTO;

namespace MedicalResearch.Api.DTOValidators
{
    public class SupplyCreateDTOValidator: AbstractValidator<SupplyCreateDTO>
    {
        public SupplyCreateDTOValidator() 
        {
            RuleFor(s => s.ClinicId)
                .NotEmpty().WithMessage("Clinic ID is required.")
                .GreaterThan(0).WithMessage("Clinic ID must be greater than 0.");
            RuleFor(s => s.MedicineId)
                .NotEmpty().WithMessage("Medicine ID is required.")
                .GreaterThan(0).WithMessage("Medicine ID must be greater than 0.");
            RuleFor(s => s.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
