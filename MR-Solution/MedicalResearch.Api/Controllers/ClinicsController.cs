using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;


namespace MedicalResearch.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ClinicsController(IMapper mapper, IClinicService clinicService) : ControllerBase
{
    // GET: api/<ClinicController>
    [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Clinic>))]
    [PageListFilter<ClinicDTO>]
    public async Task<ActionResult<List<ClinicDTO>>> GetClinics([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var clinics = await clinicService.GetClinicsAsync(query);
        var clinicDTOs = mapper.Map<List<ClinicDTO>>(clinics);
        var pagedDTO = new PagedList<ClinicDTO>(clinicDTOs, clinics.TotalCount, clinics.CurrentPage, clinics.PageSize);
        return Ok(pagedDTO);
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
