using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MedicalResearch.Domain.Models
{
    public class MedicineContainer: Entity
    {
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Medicine> Medicines { get; set; } = [];
    }
}
