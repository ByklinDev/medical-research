using MedicalResearch.Domain.Enums;

namespace MedicalResearch.Api.DTO;

public class PatientUpdateDTO
{
    public int Id { get; set; }
    public PatientStatus Status { get; set; }
 
}
