using MedicalResearch.Domain.DTO;

namespace MedicalResearch.Domain.Interfaces.Service;

public interface IEmailService
{
    Task SendAsync(EmailCreateDTO emailCreateDTO);
}