using FluentValidation;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Validations
{
    public class PatientValidator: AbstractValidator<Patient>
    {
        private readonly IClinicService _clinicService;
        private readonly IPatientService _patientService;
        public PatientValidator(IClinicService clinicService, IPatientService patientService)
        {

            _clinicService = clinicService;
            _patientService = patientService;
            
  
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now.AddYears(-18))
                .WithMessage("Patient must be at least 18 years old.");

            RuleFor(x => x.Sex)
                .NotEmpty().WithMessage("Sex is required.")
                .IsInEnum().WithMessage("Sex must be a valid enum value.");

           
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Number is required.")
                .Matches(@"^\d{3}-\d{4}$").WithMessage("Number must be in the format 000-0000.")
                .MustAsync(async(p, cancellationToken) => { return await _clinicService.GetClinicAsync(Convert.ToInt32(p.Split('-')[0])) != null; })
                .WithMessage("Number of clinic is not exist")
                .Must((p, cancellationToken) => { return patientService.GetPatientByNumber(p.Number) != null; })
                .WithMessage("Patient with this number already exists.");
        }

    }
}
