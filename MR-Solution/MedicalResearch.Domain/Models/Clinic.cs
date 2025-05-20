using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Clinic : Entiny 
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AddressOne { get; set; } = string.Empty;
        public string? AddressTwo { get; set; }
        public string? Phone { get; set; }
        public List<User> Users { get; set; } = [];
        public List<Supply> Supplies { get; set; } = [];
        public List<Patient> Patients { get; set; } = [];
        public List<ClinicStockMedicine> ClinicStockMedicines { get; set; } = [];
        public List<Visit> Visits { get; set; } = [];
    }
}