using MedicalResearch.Domain.Enums;

namespace MedicalResearch.Domain.DTO;

public class UserUpdateDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;    
    public string ConfirmPassword { get; set; } = string.Empty;
    public UserState State { get; set; } = UserState.Active;
    public int ClinicId { get; set; }
}
