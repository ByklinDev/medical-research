using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? AddressOne { get; set; }
        public string? AddressTwo { get; set; }
        [Required]
        public string? Phone { get; set; }
        public List<User> Users { get; set; } = [];
        public List<Supply> Supplies { get; set; } = [];
        public List<Patient> Patients { get; set; } = [];
        public List<ClinicStock> ClinicStocks { get; set; } = [];
    }
}
