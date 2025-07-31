namespace MedicalResearch.Api.DTO;

public class UserRoleDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;   
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
