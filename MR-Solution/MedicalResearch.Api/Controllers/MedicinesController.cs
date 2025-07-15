using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicalResearch.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MedicinesController(IMapper mapper, IMedicineService medicineService) : ControllerBase
{
    // GET: api/<MedicineController>
    [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Medicine>))]
    [PageListFilter<MedicineDTO>]
    public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetMedicines([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var medicines = await medicineService.GetMedicinesAsync(query);
        var medicineDTOs = mapper.Map<List<MedicineDTO>>(medicines);
        var pagedDTO = new PagedList<MedicineDTO>(medicineDTOs, medicines.TotalCount, medicines.CurrentPage, medicines.PageSize);
        return Ok(pagedDTO);
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
    public async Task<ActionResult<MedicineDTO>> EditMedicine(int id, [FromBody] MedicineUpdateDTO medicineDTO)
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
