using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailsController(IEmailService emailService, IValidator<EmailCreateDTO> emailValidator) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> SendAsync(EmailCreateDTO emailCreateDTO)
    {
        var resultValidation = emailValidator.Validate(emailCreateDTO);
        if (!resultValidation.IsValid)
        {
            throw new DomainException(resultValidation.Errors[0].ErrorMessage);
        }
        await emailService.SendAsync(emailCreateDTO);
        return Ok();
    }
}
