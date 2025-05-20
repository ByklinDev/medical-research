using FluentValidation;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class MedicineTypeValidator: AbstractValidator<MedicineType>
    {
        public MedicineTypeValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 50).WithMessage("Name must be between 1 and 50 characters long.");          
        }
    }
}
