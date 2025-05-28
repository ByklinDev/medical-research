using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class MedicineDTO
    {
        public int Id { get; set; } 
        public string Description { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public MedicineState State { get; set; }
        public int Amount { get; set; }
        public int MedicineTypeId { get; set; }
        public MedicineType MedicineType { get; set; } = new();
    }
}
