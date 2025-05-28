using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class VisitCreateDTO
    {
        public int ClinicId { get; set; }
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
       
    }
}
