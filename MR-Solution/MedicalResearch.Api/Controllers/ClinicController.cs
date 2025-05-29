using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using MedicalResearch.Domain.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController(IMapper mapper, IClinicService clinicService) : ControllerBase
    {
        // GET: api/<ClinicController>
        [HttpGet]
        public async Task<ActionResult<List<ClinicDTO>>> GetClinics([FromQuery] Query query)
        {
            var clinics = await clinicService.GetClinicsAsync(query);
            var clinicDTOs = mapper.Map<List<ClinicDTO>>(clinics);
            return Ok(clinicDTOs);
        }

        // GET: api/<ClinicController>
        [HttpGet("ByName")]
        public async Task<ActionResult<List<ClinicDTO>>> GetClinicsByNameAsync([FromQuery] Query query)
        {
            var clinics = await clinicService.GetClinicsByNameAsync(query);
            var clinicDTOs = mapper.Map<List<ClinicDTO>>(clinics);
            return Ok(clinicDTOs);
        }
        // GET api/<ClinicController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicDTO>> GetClinic(int id)
        {
            var clinic = await clinicService.GetClinicAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
            var clinicDTO = mapper.Map<ClinicDTO>(clinic);
            return Ok(clinicDTO);
        }

        // POST api/<ClinicController>
        [HttpPost]
        public async Task<ActionResult<ClinicDTO>> AddClinic([FromBody] ClinicCreateDTO clinicCreateDTO)
        {
            if (clinicCreateDTO == null)
            {
                return BadRequest("Clinic data is null");
            }
            var clinic = mapper.Map<Clinic>(clinicCreateDTO);
            var clinicAdded = await clinicService.AddClinicAsync(clinic);
            if (clinicAdded == null)
            {
                return BadRequest("Clinic could not be added");
            }
            var clinicDTO = mapper.Map<ClinicDTO>(clinicAdded);
            return CreatedAtAction(nameof(GetClinic), new { id = clinic.Id }, clinicDTO);
        }

        // PUT api/<ClinicController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ClinicDTO>> EditClinic(int id, [FromBody] ClinicDTO clinicDTO)
        {
            if (clinicDTO == null || clinicDTO.Id != id)
            {
                return BadRequest("Clinic data is invalid");
            }
            var clinic = mapper.Map<Clinic>(clinicDTO);
            var clinicUpdated = await clinicService.UpdateClinicAsync(clinic);
            if (clinicUpdated == null)
            {
                return NotFound(id);
            }
            var updatedClinicDTO = mapper.Map<ClinicDTO>(clinicUpdated);
            return Ok(updatedClinicDTO);
        }

        // DELETE api/<ClinicController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteClinic(int id)
        {
            var clinicDeleted = await clinicService.DeleteClinicAsync(id);
            if (!clinicDeleted)
            {
                return BadRequest("Clinic is not deleted");
            }
            return Ok(clinicDeleted);
        }
    }
}
