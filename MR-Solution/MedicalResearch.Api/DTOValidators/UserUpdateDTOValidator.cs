using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.DTO;

namespace MedicalResearch.Api.DTOValidators;

public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
{
    public UserUpdateDTOValidator()
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
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithMessage(errorMessage: "Confirm Password must match New Password.");
    }
}
