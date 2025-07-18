using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO;

public class SupplyDTO
{
    public int Id { get; set; }
    public DateTime DateArrival { get; set; }
    public int Amount { get; set; }
    public int ClinicId { get; set; }
    public int MedicineId { get; set; }
    public int UserId { get; set; }
    public bool IsActive { get; set; }
    public string MedicineDescription { get; set; } = string.Empty;
    public string ClinicName { get; set; } = string.Empty;
}
