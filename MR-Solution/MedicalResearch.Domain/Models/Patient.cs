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
    public class Patient
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public PatientStatus Status { get; set; }

    }
}
