namespace MedicalResearch.Domain.DTO;

public class EmailCreateDTO
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string Message { get; set; } = null!;
}
