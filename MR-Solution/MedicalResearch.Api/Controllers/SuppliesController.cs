using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata.Ecma335;


namespace MedicalResearch.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SuppliesController(IMapper mapper,
                              ISupplyService supplyService,
                              IValidator<Medicine> medicineValidator,
                              IValidator<Supply> supplyValidator,
                              IUserService userService,
                              IValidator<SupplyCreateDTO> supplyCreateValidator,
                              IMedicineService medicineService,
                              IClinicService clinicService) : ControllerBase
{
    // GET: api/<SupplyController>
    [HttpGet]
    [ActionName(nameof(GetSuppliesAsync))]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Supply>))]
    [PageListFilter<SupplyDTO>]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesAsync([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(null, null, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        var pagedDTO = new PagedList<SupplyDTO>(supplyDTOs, supplies.TotalCount, supplies.CurrentPage, supplies.PageSize);
        return Ok(pagedDTO);
    }

    [HttpGet("Clinics/{clinicId}")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Supply>))]
    [PageListFilter<SupplyDTO>]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByClinicAsync(int clinicId, [FromQuery] QueryDTO queryDTO)
    {
        if (clinicId <= 0)
        {
            return BadRequest("Invalid clinic ID.");
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(clinicId, null, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        var pagedDTO = new PagedList<SupplyDTO>(supplyDTOs, supplies.TotalCount, supplies.CurrentPage, supplies.PageSize);
        return Ok(pagedDTO);
    }

    [HttpGet("Medicines/{medicineId}")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Supply>))]
    [PageListFilter<SupplyDTO>]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByMedicineAsync(int medicineId, [FromQuery] QueryDTO queryDTO)
    {
        if (medicineId <= 0)
        {
            return BadRequest("Invalid medicine ID.");
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(null, medicineId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        var pagedDTO = new PagedList<SupplyDTO>(supplyDTOs, supplies.TotalCount, supplies.CurrentPage, supplies.PageSize);
        return Ok(pagedDTO);
    }

    [HttpGet("Clinics/{clinicId}/Medicines/{medicineId}")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Supply>))]
    [PageListFilter<SupplyDTO>]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSuppliesByMedicineAsync(int clinicId, int medicineId, [FromQuery] QueryDTO queryDTO)
    {
        if (clinicId <= 0 || medicineId <= 0)
        {
            return BadRequest("Invalid clinic or medicine ID.");
        }
        var query = mapper.Map<Query>(queryDTO);
        var supplies = await supplyService.GetSuppliesAsync(clinicId, medicineId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        var pagedDTO = new PagedList<SupplyDTO>(supplyDTOs, supplies.TotalCount, supplies.CurrentPage, supplies.PageSize);
        return Ok(pagedDTO);
    }

    [HttpGet("Users/{userId}")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<Supply>))]
    [PageListFilter<SupplyDTO>]
    public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetInactiveSuppliesByUserIdAsync(int userId, [FromQuery] QueryDTO queryDTO)
    {
        if (userId <= 0)
        {
            return BadRequest("Invalid user ID.");
        }
        var query = mapper.Map<Query>(queryDTO);
        var user = await userService.GetUserAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var supplies = await supplyService.GetInactiveSuppliesByUserIdAsync(userId, query);
        var supplyDTOs = mapper.Map<List<SupplyDTO>>(supplies);
        
        var pagedDTO = new PagedList<SupplyDTO>(supplyDTOs, supplies.TotalCount, supplies.CurrentPage, supplies.PageSize);
        return Ok(pagedDTO);
    }


    // GET api/<SupplyController>/5
    [ActionName(nameof(GetSupplyAsync))]
    [HttpGet("{id}")]
    public async Task<ActionResult<SupplyDTO>> GetSupplyAsync(int id)
    {
        var supply = await supplyService.GetSupplyAsync(id);
        if (supply == null)
        {
            throw new DomainException($"Not found supply with id: {id}");
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
            throw new DomainException(resultSupplyCreateValidation.Errors[0].ErrorMessage);
        }
        var medicine = await medicineService.GetMedicineAsync(supplyCreateDTO.MedicineId);
        if (medicine == null)
        {
            throw new DomainException($"Medicine id: {supplyCreateDTO.MedicineId} not found");
        }
        var resultMedicineValidation = await medicineValidator.ValidateAsync(medicine);
        if (!resultMedicineValidation.IsValid)
        {
            throw new DomainException(resultMedicineValidation.Errors[0].ErrorMessage);
        }
        var supply = await supplyService.AddToSupply(medicine, supplyCreateDTO.Amount, supplyCreateDTO.ClinicId, supplyCreateDTO.UserId);
        var resultSupplyValidation = await supplyValidator.ValidateAsync(supply);
        if (!resultSupplyValidation.IsValid)
        {
            throw new DomainException(resultSupplyValidation.Errors[0].ErrorMessage);
        }
        supply.Medicine = medicine;
        var clinic = await clinicService.GetClinicAsync(supply.ClinicId);
        if (clinic != null)
        {
            supply.Clinic = clinic;
        }
        var supplyDTO = mapper.Map<SupplyDTO>(supply);
        
        return CreatedAtAction(nameof(GetSupplyAsync), new { id = supplyDTO.Id}, supplyDTO);
    }


    [HttpPatch("{userId}")]
    public async Task<ActionResult<SupplyDTO>> AddSupply(int userId, [FromBody] List<SupplyDTO> supplyDTOs)
    {
        var supplies = supplyDTOs.Select(mapper.Map<SupplyDTO, Supply>).ToList();
        var addedSupplies = await supplyService.AddSupplyAsync(supplies, userId);
        var supplyDTOsResult = addedSupplies.Select(mapper.Map<Supply, SupplyDTO>).ToList();
        if (supplyDTOsResult == null || !supplyDTOsResult.Any())
        {
            throw new DomainException("No supplies were added.");
        }
        return CreatedAtAction(nameof(GetSuppliesAsync), supplyDTOsResult);
    }

    // PUT api/<SupplyController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<SupplyDTO>> EditSupply(int id, [FromBody] SupplyDTO supplyDTO)
    {
        if (id != supplyDTO.Id)
        {
            throw new DomainException($"Supply ID mismatch {id} vs {supplyDTO.Id}.");
        }
        var supply = mapper.Map<Supply>(supplyDTO);
        var resultSupplyValidation = supplyValidator.Validate(supply);
        if (!resultSupplyValidation.IsValid)
        {
            throw new DomainException(resultSupplyValidation.Errors[0].ErrorMessage);
        }
        var updatedSupply = await supplyService.UpdateSupplyAsync(supply);
        if (updatedSupply == null)
        {
            throw new DomainException("Supply was not updated");
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
            throw new DomainException($"Supply with id: {id} not found");
        }
        var isDeleted = await supplyService.DeleteSupplyAsync(id);
        if (!isDeleted)
        {
            throw new DomainException("Failed to delete supply.");
        }
        return Ok(isDeleted);
    }
}
