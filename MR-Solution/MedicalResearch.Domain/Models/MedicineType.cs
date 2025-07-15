
using System.Text.Json.Serialization;

namespace MedicalResearch.Domain.Models
{
    public class MedicineType: Entity
    {
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Medicine> Medicines { get; set; } = [];
    }
}
