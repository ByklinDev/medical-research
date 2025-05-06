using FluentValidation;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class ClinicValidator: AbstractValidator<Clinic>
    {
        public ClinicValidator() 
        { 
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(2, 50)
                .WithMessage("Name must be between 2 and 50 characters long.");

            RuleFor(x => x.AddressOne)
                .NotEmpty()
                .WithMessage("Address one is required.")
                .Length(2, 100)
                .WithMessage("Address one must be between 2 and 100 characters long.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .Length(2, 50)
                .WithMessage("City must be between 2 and 50 characters long.");
        }
    }
}