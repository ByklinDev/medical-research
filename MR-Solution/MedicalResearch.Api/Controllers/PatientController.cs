using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IMapper mapper, IPatientService patientService ) : ControllerBase
    {
        // GET: api/<PatientController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients([FromQuery] Query query)
        {
            var patients = await patientService.GetPatientsAsync(query);
            var patientDTOs = mapper.Map<List<PatientDTO>>(patients);
            return Ok(patientDTOs);
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await patientService.GetPatientAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            var patientDTO = mapper.Map<PatientDTO>(patient);
            return Ok(patientDTO);
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
        public async Task<ActionResult<PatientDTO>> EditPatient(int id, [FromBody] PatientDTO patientDTO)
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
            var updatedPatientDTO = mapper.Map<PatientDTO>(updatedPatient);
            return Ok(updatedPatientDTO);
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
}
