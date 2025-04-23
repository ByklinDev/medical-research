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
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Initials { get; set; }
        [Required]       
        public string? Email { get; set; }
        public UserState State { get; set; }
        public string? Password { get; set; }
        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public List<Role> Roles { get; set; } = [];
    }
}
