using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;


namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class MedicineTypeController(IMapper mapper, IServiceProvider serviceProvider, IMedicineTypeService medicineTypeService) : ControllerBase
{
    // GET: api/<MedicineTypeController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicineTypeDTO>>> GetMedicineTypes([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<MedicineType>));
        if (validator == null) 
        {
            return BadRequest("No suitable validator found for QueryDTO<MedicineType>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var medicineTypes = await medicineTypeService.GetMedicineTypesAsync(query);
        var medicineTypeDTOs = mapper.Map<List<MedicineTypeDTO>>(medicineTypes);
        return Ok(medicineTypeDTOs);
    }

    // GET api/<MedicineTypeController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineTypeDTO>> GetMedicineType(int id)
    {
        var medicineType = await medicineTypeService.GetMedicineTypeAsync(id);
        if (medicineType == null)
        {
            return NotFound(id);
        }
        var medicineTypeDTO = mapper.Map<MedicineTypeDTO>(medicineType);
        return Ok(medicineTypeDTO);
    }

    // POST api/<MedicineTypeController>
    [HttpPost]
    public async Task<ActionResult<MedicineTypeDTO>> AddMedicineType([FromBody] MedicineTypeCreateDTO medicineTypeCreateDTO )
    {
        if (medicineTypeCreateDTO == null)
        {
            return BadRequest("Medicine type data is null");
        }
        var medicineType = mapper.Map<MedicineType>(medicineTypeCreateDTO);
        var medicineTypeAdded = await medicineTypeService.AddMedicineTypeAsync(medicineType);
        if (medicineTypeAdded == null)
        {
            return BadRequest("Medicine type could not be added");
        }
        var medicineTypeDTO = mapper.Map<MedicineTypeDTO>(medicineTypeAdded);
        return CreatedAtAction(nameof(GetMedicineType), new { id = medicineType.Id }, medicineTypeDTO);
    }

    // PUT api/<MedicineTypeController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<MedicineTypeDTO>> EditMedicineType(int id, [FromBody] MedicineTypeDTO medicineTypeDTO)
    {
        if (medicineTypeDTO == null || medicineTypeDTO.Id != id)
        {
            return BadRequest("Id mismatch");
        }
        var medicineType = mapper.Map<MedicineType>(medicineTypeDTO);
        var updatedMedicineType = await medicineTypeService.UpdateMedicineTypeAsync(medicineType);
        if (updatedMedicineType == null)
        {
            return NotFound($"Medicine type {id} is not updated");
        }
        var updatedMedicineTypeDTO = mapper.Map<MedicineTypeDTO>(updatedMedicineType);
        return Ok(updatedMedicineTypeDTO);
    }

    // DELETE api/<MedicineTypeController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteMedicineType(int id)
    {
        var deleted = await medicineTypeService.DeleteMedicineTypeAsync(id);
        if (!deleted)
        {
            return NotFound($"Medicine type {id} not found");
        }
        return Ok(deleted);
    }
}
