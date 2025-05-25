namespace MedicalResearch.Api.DTO
{
    public class UserDTO
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? ClinicId { get; set; }
    }
}
