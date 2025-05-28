using FluentValidation;
using MedicalResearch.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class CreateUserDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(x => x.FirstName)
                            .NotEmpty()
                            .WithMessage("First name is required.")
                            .Length(2, 50)
                            .WithMessage("First name must be between 2 and 50 characters.")
                            .Must(s => !s.Any(char.IsDigit))
                            .WithMessage("First Name should not contain any numbers.");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .Length(2, 50)
                .WithMessage("Last name must be between 2 and 50 characters.")
                .Must(s => !s.Any(char.IsDigit))
                .WithMessage("Last Name should not contain any numbers.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(x => x.Initials)
                .Must(s => s.Any(char.IsLetter))
                .WithMessage("Initials should contain at least one letter.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(8, 100)
                .WithMessage("Password must be between 8 and 100 characters.");
            RuleFor(x => x.PasswordRepeat)
                .NotEmpty()
                .WithMessage("Password repeat is required.")
                .Equal(x => x.Password)
                .WithMessage("Password and Password repeat must match.");
        }
    }
}
