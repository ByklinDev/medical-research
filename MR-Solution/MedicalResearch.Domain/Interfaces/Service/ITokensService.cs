using MedicalResearch.Domain.Models;

namespace MedicalResearch.Domain.Interfaces.Service;

public interface ITokensService
{
    (string token, DateTime expiration) GenerateAccessToken(User user);
}