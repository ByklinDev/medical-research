using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.DTO;

public class PatientResearchDTO
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime LastVisitDate { get; set; }
    public string Medicines { get; set; } = string.Empty;
    public int ClinicId { get; set; }
    public Sex Sex { get; set; }
    public PatientStatus Status { get; set; }
}
