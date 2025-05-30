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
public class MedicineController(IMapper mapper, IServiceProvider serviceProvider, IMedicineService medicineService) : ControllerBase
{
    // GET: api/<MedicineController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetMedicines([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Medicine>));
        if (validator == null) 
        {
            return BadRequest("No suitable validator found for QueryDTO<Medicine>.");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var medicines = await medicineService.GetMedicinesAsync(query);
        var medicineDTOs = mapper.Map<List<MedicineDTO>>(medicines);
        return Ok(medicineDTOs);
    }

    // GET api/<MedicineController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineDTO>> GetMedicine(int id)
    {
        var medicine = await medicineService.GetMedicineAsync(id);
        if (medicine == null)
        {
            return NotFound();
        }
        var medicineDTO = mapper.Map<MedicineDTO>(medicine);
        return Ok(medicineDTO);
    }

    // POST api/<MedicineController>
    [HttpPost]
    public async Task<ActionResult<MedicineDTO>> AddMedicine([FromBody] MedicineCreateDTO medicineCreateDTO)
    {
        var medicine = mapper.Map<Domain.Models.Medicine>(medicineCreateDTO);
        var medicineAdded = await medicineService.AddMedicineAsync(medicine);
        if (medicineAdded == null)
        {
            return BadRequest("Medicine could not be added");
        }
        var medicineDTO = mapper.Map<MedicineDTO>(medicineAdded);
        return CreatedAtAction(nameof(GetMedicine), new { id = medicineAdded.Id }, medicineDTO);
    }

    // PUT api/<MedicineController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<MedicineDTO>> EditMedicine(int id, [FromBody] MedicineDTO medicineDTO)
    {
        if (medicineDTO == null || medicineDTO.Id != id)
        {
            return BadRequest("Medicine data is null or ID mismatch");
        }
        var medicine = mapper.Map<Domain.Models.Medicine>(medicineDTO);
        var updatedMedicine = await medicineService.UpdateMedicineAsync(medicine);
        if (updatedMedicine == null)
        {
            return NotFound("Medicine not found");
        }
        var updatedMedicineDTO = mapper.Map<MedicineDTO>(updatedMedicine);
        return Ok(updatedMedicineDTO);
    }

    // DELETE api/<MedicineController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteMedicine(int id)
    {
        var result = await medicineService.DeleteMedicineAsync(id);
        if (!result)
        {
            return NotFound("Medicine not found");
        }
        return Ok(result);
    }
}
