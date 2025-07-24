using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace MedicalResearch.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatientsController(IMapper mapper, IPatientService patientService ) : ControllerBase
{
    // GET: api/<PatientController>
    [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Patient>))]
    [PageListFilter<PatientDTO>]
    public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var patients = await patientService.GetPatientsInfo(query);
        return Ok(patients);
    }

    // GET api/<PatientController>/5
    [ActionName(nameof(GetPatient))]
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResearchDTO>> GetPatient(int id)
    {
        var patient = await patientService.GetPatientInfo(id);
        if (patient == null)
        {
            return NotFound();
        }
        return Ok(patient);
    }

    // POST api/<PatientController>
    [HttpPost]
    public async Task<ActionResult<PatientDTO>> AddPatient([FromBody] PatientCreateDTO patientCreateDTO)
    {
        var patient = mapper.Map<Patient>(patientCreateDTO);
        var patientAdded = await patientService.AddPatientAsync(patient);
        if (patientAdded == null)
        {
            return BadRequest("Patient could not be added");
        }
        var patientDTO = mapper.Map<PatientDTO>(patientAdded);
        return CreatedAtAction(nameof(GetPatient), new { id = patientAdded.Id }, patientDTO);
    }

    // PUT api/<PatientController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResearchDTO>> EditPatient(int id, [FromBody] PatientUpdateDTO patientDTO)
    {
        if (patientDTO == null || patientDTO.Id != id)
        {
            return BadRequest("Patient data is null or ID mismatch");
        }
        var patient = mapper.Map<Patient>(patientDTO);
        var updatedPatient = await patientService.UpdatePatientAsync(patient);
        if (updatedPatient == null)
        {
            return NotFound("Patient not found");
        }
        return Ok(updatedPatient);
    }

    // DELETE api/<PatientController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeletePatient(int id)
    {
        var deleted = await patientService.DeletePatientAsync(id);
        if (!deleted)
        {
            return NotFound("Patient not found");
        }
        return Ok(true);
    }
}
