using FluentValidation;
using MedicalResearch.Domain.DTO;

namespace MedicalResearch.Api.DTOValidators;

public class EmailCreateDTOValidator : AbstractValidator<EmailCreateDTO>
{
    public EmailCreateDTOValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(x => x.Topic).NotEmpty().WithMessage("Topic is required.");
        RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
