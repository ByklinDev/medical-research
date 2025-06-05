
namespace MedicalResearch.Domain.Models
{
    public class Role: Entity
    {
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = [];
    }
}
