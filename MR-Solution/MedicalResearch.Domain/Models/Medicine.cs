using MedicalResearch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Medicine: Entity
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; } = DateTime.UtcNow.AddMonths(12);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Amount { get; set; }
        public int MedicineTypeId { get; set; }
        public MedicineType MedicineType { get; set; } = new ();
        public int MedicineContainerId { get; set; }
        public MedicineContainer MedicineContainer { get; set; } = new ();
        public int DosageFormId { get; set; }
        public DosageForm DosageForm { get; set; } = new ();
        public MedicineState State { get; set; }
        public List<Supply> Supplies { get; set; } = [];
    }
}