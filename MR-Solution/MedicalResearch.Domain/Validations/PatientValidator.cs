using FluentValidation;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class PatientValidator: AbstractValidator<Patient>
    {
        public PatientValidator() 
        {
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now.AddYears(-18))
                .WithMessage("Patient must be at least 18 years old.");

            RuleFor(x => x.Sex)
                .NotEmpty().WithMessage("Sex is required.")
                .IsInEnum().WithMessage("Sex must be a valid enum value.");
        }
    }
}
