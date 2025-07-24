using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public PatientStatus Status { get; set; }
        public DateTime LastVisitDate {  get; set; }
        public string Medicines { get; set; } = string.Empty;
    }
}
