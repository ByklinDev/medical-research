using MedicalResearch.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalResearch.Api.DTO
{
    public class UserCreateDTO
    {       
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

    }
}
