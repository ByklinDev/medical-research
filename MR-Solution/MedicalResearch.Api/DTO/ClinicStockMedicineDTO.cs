using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class ClinicStockMedicineDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ClinicId { get; set; }
        public int MedicineId { get; set; }
    }
}
