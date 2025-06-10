using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicineContainersController(IMapper mapper, IMedicineContainerService medicineContainerService) : ControllerBase
{
    // GET: api/<MedicineContainerController>
    [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<MedicineContainer>))]
    [PageListFilter<MedicineContainerDTO>]
    public async Task <ActionResult<IEnumerable<MedicineContainerDTO>>> GetMedicineContainers([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var medicineContainers = await medicineContainerService.GetMedicineContainersAsync(query);       
        var medicineContainerDTOs = mapper.Map<List<MedicineContainerDTO>>(medicineContainers);
        var pagedDTO = new PagedList<MedicineContainerDTO>(medicineContainerDTOs, medicineContainers.TotalCount, medicineContainers.CurrentPage, medicineContainers.PageSize );
        return Ok(pagedDTO);
    }

    // GET api/<MedicineContainerController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineContainerDTO>> GetMedicineContainer(int id)
    {
        var medicineContainer = await medicineContainerService.GetMedicineContainerAsync(id);
        if (medicineContainer == null)
        {
            return NotFound(id);
        }
        var medicineContainerDTO = mapper.Map<MedicineContainerDTO>(medicineContainer);
        return Ok(medicineContainerDTO);
    }

    // POST api/<MedicineContainerController>
    [HttpPost]
    public async Task<ActionResult<MedicineContainerDTO>> AddMedicineContainer([FromBody] MedicineContainerCreateDTO medicineContainerCreateDTO)
    {
        if (medicineContainerCreateDTO == null)
        {
            return BadRequest("Medicine container data is null");
        }
        var medicineContainer = mapper.Map<MedicineContainer>(medicineContainerCreateDTO);
        var medicineContainerAdded = await medicineContainerService.AddMedicineContainerAsync(medicineContainer);
        if (medicineContainerAdded == null)
        {
            return BadRequest("Medicine container could not be added");
        }
        var medicineContainerDTO = mapper.Map<MedicineContainerDTO>(medicineContainerAdded);
        return CreatedAtAction(nameof(GetMedicineContainer), new { id = medicineContainer.Id }, medicineContainerDTO);
    }

    // PUT api/<MedicineContainerController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<MedicineContainerDTO>> EditMedicineContainer(int id, [FromBody] MedicineContainerDTO medicineContainerDTO)
    {
        if (medicineContainerDTO == null || medicineContainerDTO.Id != id)
        {
            return BadRequest("Id mismatch");
        }
        var medicineContainer = mapper.Map<MedicineContainer>(medicineContainerDTO);
        var updatedMedicineContainer = await medicineContainerService.UpdateMedicineContainerAsync(medicineContainer);
        if (updatedMedicineContainer == null)
        {
            return NotFound("Medicine container is not updated");
        }
        var updatedMedicineContainerDTO = mapper.Map<MedicineContainerDTO>(updatedMedicineContainer);
        return Ok(updatedMedicineContainerDTO);
    }

    // DELETE api/<MedicineContainerController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteMedicineContainer(int id)
    {
        var isDeleted = await medicineContainerService.DeleteMedicineContainerAsync(id);
        if (!isDeleted)
        {
            return NotFound("Medicine container is not deleted");
        }
        return Ok(isDeleted);
    }
}
