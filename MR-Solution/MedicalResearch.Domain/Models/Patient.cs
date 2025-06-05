using MedicalResearch.Domain.Enums;


namespace MedicalResearch.Domain.Models
{
    public class Patient: Entity
    {
        public string Number { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = new ();
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public PatientStatus Status { get; set; }
        public List<Visit> Visits { get; set; } = [];
    }
}