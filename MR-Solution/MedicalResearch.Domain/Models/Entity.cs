using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicalResearch.Domain.Models
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
