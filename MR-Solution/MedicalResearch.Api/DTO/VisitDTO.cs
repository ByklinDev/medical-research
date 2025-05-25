using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public int MedicineId { get; set; }
        public int NumberOfVisit { get; set; }
        public int UserId { get; set; }
    }
}
