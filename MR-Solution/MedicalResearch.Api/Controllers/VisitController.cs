using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;


namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class VisitController(IMapper mapper, IServiceProvider serviceProvider, IVisitService visitService) : ControllerBase
{
    // GET: api/<VisitController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitDTO>>> GetVisits([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                        .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Visit>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Visit>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var visits = await visitService.GetVisitsAsync(query);
        var visitDTOs = mapper.Map<List<VisitDTO>>(visits);
        return Ok(visitDTOs);
    }


    // GET api/<VisitController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitDTO>> GetVisit(int id)
    {
        var visit = await visitService.GetVisitAsync(id);
        if (visit == null)
        {
            return NotFound();
        }
        var visitDTO = mapper.Map<VisitDTO>(visit);
        return Ok(visitDTO);
    }

    // POST api/<VisitController>
    [HttpPost]
    public async Task<ActionResult<VisitDTO>> AddVisit([FromBody] VisitDTO visitDTO)
    {
        if (visitDTO == null)
        {
            return BadRequest("Visit data is null");
        }
        var visit = mapper.Map<Visit>(visitDTO);
        var createdVisit = await visitService.AddVisitAsync(visit);
        if (createdVisit == null)
        {
            return BadRequest("Visit not created");
        }
        return CreatedAtAction(nameof(GetVisit), new { id = createdVisit.Id }, mapper.Map<VisitDTO>(createdVisit));
    }

    // PUT api/<VisitController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<VisitDTO>> EditVisit(int id, [FromBody] VisitDTO visitDTO)
    {
        if (visitDTO == null || visitDTO.Id != id)
        {
            return BadRequest("Visit data is invalid");
        }
        var existingVisit = await visitService.GetVisitAsync(id);
        if (existingVisit == null)
        {
            return NotFound();
        }
        var visit = mapper.Map<Visit>(visitDTO);
        var updatedVisit = await visitService.UpdateVisitAsync(visit);
        if (updatedVisit == null)
        {
            return BadRequest("Visit not updated");
        }
        return Ok(mapper.Map<VisitDTO>(updatedVisit));
    }

    // DELETE api/<VisitController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteVisit(int id)
    {
        var isDeleted = await visitService.DeleteVisitAsync(id);
        if (!isDeleted)
        {
            return NotFound($"Visit with ID {id} not found or could not be deleted.");
        }
        return Ok(true);
    }
}
