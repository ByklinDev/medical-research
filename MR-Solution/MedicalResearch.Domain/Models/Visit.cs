using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Visit
    {
       
        public int ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public DateTime DateOfVisit { get; set; }
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public int NumberOfVisit { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
