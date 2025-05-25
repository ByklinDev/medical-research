using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class PatientCreateDTO
    {
        public string Number { get; set; } = string.Empty;
  
        public DateTime DateOfBirth { get; set; }

        public Sex Sex { get; set; }

    }
}
