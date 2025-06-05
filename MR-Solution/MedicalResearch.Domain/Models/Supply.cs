namespace MedicalResearch.Domain.Models
{
    public class Supply: Entity
    {
        public DateTime DateArrival { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; } = false;
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = new();
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = new();
        public int UserId { get; set; }
        public User User { get; set; } = new();
        public List<ClinicStockMedicine> ClinicStockMedicines { get; set; } = [];
    }
}
