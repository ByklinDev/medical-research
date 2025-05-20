using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Visit: Entity
    {
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = new ();
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = new ();
        public DateTime DateOfVisit { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = new ();
        public int NumberOfVisit { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new ();
    }
}