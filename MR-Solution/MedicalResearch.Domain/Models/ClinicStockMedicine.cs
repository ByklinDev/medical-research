using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class ClinicStockMedicine: Entiny
    {
        public int Amount { get; set; }
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = new ();
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = new ();
        public List<Supply> Supplies { get; set; } = [];
    }
}