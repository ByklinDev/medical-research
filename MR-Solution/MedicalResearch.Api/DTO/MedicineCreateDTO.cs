using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class MedicineCreateDTO
    {
        public string Description { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
        public int Amount { get; set; }
        public int MedicineTypeId { get; set; }
        public int MedicineContainerId { get; set; }
        public int DosageFormId { get; set; }
    }
}
