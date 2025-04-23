using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Supply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime? DateArrival { get; set; }
        public int Amount { get; set; }
        [Required]
        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        [Required]
        public int? MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public List<ClinicStock> ClinicStocks { get; set; } = [];
    }
}
