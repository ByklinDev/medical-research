using FluentValidation;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class RoleValidator: AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Role name is required.")
                .Length(2, 50)
                .WithMessage("Role name must be between 2 and 50 characters.");
        }
    }
}