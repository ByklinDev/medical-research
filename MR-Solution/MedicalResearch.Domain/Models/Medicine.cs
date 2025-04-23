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
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime? ExpireAt { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        public int? Amount { get; set; }
        public int MedicineTypeId { get; set; }
        public MedicineType? MedicineType { get; set; }
        public int ContainerId { get; set; }
        public Container? Container { get; set; }
        public int DosageFormId { get; set; }
        public DosageForm? DosageForm { get; set; }
        public MedicineState State { get; set; }
        public List<Supply> Supplies { get; set; } = [];
    }
}
