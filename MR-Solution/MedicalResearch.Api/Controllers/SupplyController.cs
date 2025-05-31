using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;


namespace MedicalResearch.Api.Controllers;

[Route("api/Supplies")]
[ApiController]
public class SupplyController(IMapper mapper,
                              ISupplyService supplyService,
                              IServiceProvider serviceProvider,
                              IValidator<Medicine> medicineValidator,
                              IValidator<Supply> supplyValidator,
                              IUserService userService,
                              IValidator<SupplyCreateDTO> supplyCreateValidator,
                              IMedicineService medicineService) : ControllerBase
{
    // GET: api/<SupplyController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesAsync([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Supply>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Supply>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(null, null, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        return Ok(supplyDTOs);
    }

    [HttpGet("Clinics/{clinicId}")]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByClinicAsync(int clinicId, [FromQuery] QueryDTO queryDTO)
    {
        if (clinicId <= 0)
        {
            return BadRequest("Invalid clinic ID.");
        }
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Supply>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Supply>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(clinicId, null, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        return Ok(supplyDTOs);
    }

    [HttpGet("Medicines/{medicineId}")]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByMedicineAsync(int medicineId, [FromQuery] QueryDTO queryDTO)
    {
        if (medicineId <= 0)
        {
            return BadRequest("Invalid medicine ID.");
        }
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Supply>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Supply>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(null, medicineId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        return Ok(supplyDTOs);
    }

    [HttpGet("Clinics/{clinicId}/Medicines/{medicineId}")]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByMedicineAsync(int clinicId, int medicineId, [FromQuery] QueryDTO queryDTO)
    {
        if (clinicId <= 0 || medicineId <= 0)
        {
            return BadRequest("Invalid clinic or medicine ID.");
        }
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Supply>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Supply>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(clinicId, medicineId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        return Ok(supplyDTOs);
    }

    [HttpGet("Users/{userId}")]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetInactiveSuppliesByUserIdAsync(int userId, [FromQuery] QueryDTO queryDTO)
    {
        if (userId <= 0)
        {
            return BadRequest("Invalid user ID.");
        }
        var validator = serviceProvider.GetServices<IValidator<QueryDTO>>()
                       .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<Supply>));
        if (validator == null)
        {
            return BadRequest("No suitable validator found for QueryDTO<Supply>");
        }
        var validationResult = await validator.ValidateAsync(queryDTO);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        var query = mapper.Map<Query>(queryDTO);
        var user = await userService.GetUserAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var supplies = await supplyService.GetInactiveSuppliesByUserIdAsync(userId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        return Ok(supplyDTOs);
    }


    // GET api/<SupplyController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SupplyDTO>> GetSupply(int id)
    {
        var supply = await supplyService.GetSupplyAsync(id);
        if (supply == null)
        {
            return NotFound();
        }
        var supplyDTO = mapper.Map<SupplyDTO>(supply);
        return Ok(supplyDTO);
    }

    // POST api/<SupplyController>
    [HttpPost]
    public async Task<ActionResult<SupplyDTO>> AddToSupply([FromBody] SupplyCreateDTO supplyCreateDTO)
    {
        var resultSupplyCreateValidation = supplyCreateValidator.Validate(supplyCreateDTO);
        if (!resultSupplyCreateValidation.IsValid)
        {
            return BadRequest(resultSupplyCreateValidation.Errors);
        }
        var medicine = await medicineService.GetMedicineAsync(supplyCreateDTO.MedicineId);
        if (medicine == null)
        {
            return NotFound("Medicine not found");
        }
        var resultMedicineValidation = medicineValidator.Validate(medicine);
        if (!resultMedicineValidation.IsValid)
        {
            return BadRequest(resultMedicineValidation.Errors);
        }
        var supply = await supplyService.AddToSupply(medicine, supplyCreateDTO.Amount, supplyCreateDTO.ClinicId, supplyCreateDTO.UserId);
        var resultSupplyValidation = supplyValidator.Validate(supply);
        if (!resultSupplyValidation.IsValid)
        {
            return BadRequest(resultSupplyValidation.Errors);
        }
        var supplyDTO = mapper.Map<SupplyDTO>(supply);
        return CreatedAtAction(nameof(GetSupply), new { id = supplyDTO.Id });
    }


    [HttpPatch("{userId}")]
    public async Task<ActionResult<SupplyDTO>> AddSupply(int userId, [FromBody] List<SupplyDTO> supplyDTOs)
    {
        var supplies = supplyDTOs.Select(mapper.Map<SupplyDTO, Supply>).ToList();
        var addedSupplies = await supplyService.AddSupplyAsync(supplies, userId);
        var supplyDTOsResult = addedSupplies.Select(mapper.Map<Supply, SupplyDTO>).ToList();
        if (supplyDTOsResult == null || !supplyDTOsResult.Any())
        {
            return BadRequest("No supplies were added.");
        }
        return CreatedAtAction(nameof(GetSuppliesAsync), supplyDTOsResult);
    }

    // PUT api/<SupplyController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<SupplyDTO>> EditSupply(int id, [FromBody] SupplyDTO supplyDTO)
    {
        if (id != supplyDTO.Id)
        {
            return BadRequest("Supply ID mismatch.");
        }
        var supply = mapper.Map<Supply>(supplyDTO);
        var resultSupplyValidation = supplyValidator.Validate(supply);
        if (!resultSupplyValidation.IsValid)
        {
            return BadRequest(resultSupplyValidation.Errors);
        }
        var updatedSupply = await supplyService.UpdateSupplyAsync(supply);
        if (updatedSupply == null)
        {
            return NotFound();
        }
        var updatedSupplyDTO = mapper.Map<SupplyDTO>(updatedSupply);
        return Ok(updatedSupplyDTO);
    }

    // DELETE api/<SupplyController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteSupply(int id)
    {
        var supply = await supplyService.GetSupplyAsync(id);
        if (supply == null)
        {
            return NotFound();
        }
        var isDeleted = await supplyService.DeleteSupplyAsync(id);
        if (!isDeleted)
        {
            return BadRequest("Failed to delete supply.");
        }
        return Ok(isDeleted);
    }
}
