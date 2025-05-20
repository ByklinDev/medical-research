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
    public class User: Entiny
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        [Required]       
        public string Email { get; set; } = string.Empty;   
        public UserState State { get; set; }
        public string Password { get; set; } = string.Empty;
        public byte[] PaswordSalt { get; set; } = [];
        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public List<Role> Roles { get; set; } = [];
    }
}
