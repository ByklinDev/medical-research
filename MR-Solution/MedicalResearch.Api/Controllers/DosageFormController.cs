using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using FluentValidation;
using MedicalResearch.Api.DTOValidators;


namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class DosageFormController(IMapper mapper, IServiceProvider serviceProvider, IDosageFormService dosageFormService) : ControllerBase
{
    // GET: api/<DosageFormController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DosageFormDTO>>> GetDosageForms([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider
                        .GetServices<IValidator<QueryDTO>>()
                        .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<DosageForm>));
        if (validator == null)
        {
            return BadRequest("Validator for QueryDTO<DosageForm> not found.");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var dosageForms = await dosageFormService.GetDosageFormsAsync(query);
        var dosageFormDTOs = mapper.Map<List<DosageFormDTO>>(dosageForms);
        return Ok(dosageFormDTOs);
    }

    // GET api/<DosageFormController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DosageFormDTO>> GetDosageForm(int id)
    {
        var dosageForm = await dosageFormService.GetDosageFormAsync(id);
        if (dosageForm == null)
        {
            return NotFound(id);
        }
        var dosageFormDTO = mapper.Map<DosageFormDTO>(dosageForm);
        return Ok(dosageFormDTO);
    }

    // POST api/<DosageFormController>
    [HttpPost]
    public async Task<ActionResult<DosageFormDTO>> AddDosageForm([FromBody] DosageFormCreateDTO dosageFormCreateDTO)
    {
        if (dosageFormCreateDTO == null)
        {
            return BadRequest("Dosage form data is null");
        }
        var dosageForm = mapper.Map<DosageForm>(dosageFormCreateDTO);
        var dosageFormAdded = await dosageFormService.AddDosageFormAsync(dosageForm);
        if (dosageFormAdded == null)
        {
            return BadRequest("Dosage form could not be added");
        }
        var dosageFormDTO = mapper.Map<DosageFormDTO>(dosageFormAdded);
        return CreatedAtAction(nameof(GetDosageForm), new { id = dosageForm.Id }, dosageFormDTO);
    }

    // PUT api/<DosageFormController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<DosageFormDTO>> EditDosageForm (int id, [FromBody] DosageFormDTO dosageFormDTO)
    {
        if (dosageFormDTO == null || dosageFormDTO.Id != id)
        {
            return BadRequest("Id mismatch");
        }
        var dosageForm = mapper.Map<DosageForm>(dosageFormDTO);
        var updatedDosageForm = await dosageFormService.UpdateDosageFormAsync(dosageForm);
        if (updatedDosageForm == null)
        {
            return NotFound(id);
        }
        var updatedDosageFormDTO = mapper.Map<DosageFormDTO>(updatedDosageForm);
        return Ok(updatedDosageFormDTO);
    }

    // DELETE api/<DosageFormController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteDosageForm(int id)
    {
        var deleted = await dosageFormService.DeleteDosageFormAsync(id);
        if (!deleted)
        {
            return BadRequest("Dosage form is not deleted");
        }
        return Ok(deleted);
    }
}
