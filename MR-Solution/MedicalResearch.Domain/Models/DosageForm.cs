
namespace MedicalResearch.Domain.Models
{
    public class DosageForm: Entity
    {
        public string Name { get; set; } = string.Empty;
        public List<Medicine> Medicines { get; set; } = [];
    }
}
