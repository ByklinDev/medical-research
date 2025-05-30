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
public class ClinicStockMedicineController(IMapper mapper, IServiceProvider serviceProvider, IClinicStockMedicineService clinicStockMedicineService) : ControllerBase
{
    // GET: api/<ClinicStockMedicineController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClinicStockMedicineDTO>>> GetClinicStockMedicines([FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider
                        .GetServices<IValidator<QueryDTO>>()
                        .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<ClinicStockMedicine>));
        if (validator == null)
        {
            return BadRequest("Validator for QueryDTO<ClinicStockMedicine> not found.");
        }
        var resultValidation = await validator.ValidateAsync(queryDTO);
        if (!resultValidation.IsValid)
        {
            return BadRequest(resultValidation.Errors.Select(e => e.ErrorMessage));
        }
        var query = mapper.Map<Query>(queryDTO);
        var clinicStockMedicines = await clinicStockMedicineService.GetClinicStockMedicinesAsync(query);
        var clinicStockMedicineDTOs = mapper.Map<List<ClinicStockMedicineDTO>>(clinicStockMedicines);
        return Ok(clinicStockMedicineDTOs);
    }

    // GET api/<ClinicStockMedicineController>/5
    [HttpGet("Clinic/{clinicId}")]
    public async Task<ActionResult<IEnumerable<ClinicStockMedicineDTO>>> GetClinicStockMedicinesByClinicIdAsync(int clinicId, [FromQuery] QueryDTO queryDTO)
    {
        var validator = serviceProvider
                        .GetServices<IValidator<QueryDTO>>()
                        .FirstOrDefault(o => o.GetType() == typeof(QueryDTOValidator<ClinicStockMedicine>));
        if (validator == null)
        {
            return BadRequest("Validator for QueryDTO<ClinicStockMedicine> not found.");
        }
        var resultValidation = await validator.ValidateAsync(queryDTO);
        if (!resultValidation.IsValid)
        {
            return BadRequest(resultValidation.Errors.Select(e => e.ErrorMessage));
        }
        var query = mapper.Map<Query>(queryDTO);
        var clinicStockMedicines = await clinicStockMedicineService.GetClinicStockMedicinesByClinicIdAsync(clinicId, query);
        var clinicStockMedicineDTOs = mapper.Map<List<ClinicStockMedicineDTO>>(clinicStockMedicines);
        return Ok(clinicStockMedicineDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClinicStockMedicineDTO>> GetClinicStockMedicineByIdAsync(int id)
    {
        var clinicStockMedicine = await clinicStockMedicineService.GetClinicStockMedicineByIdAsync(id);
        if (clinicStockMedicine == null)
        {
            return NotFound();
        }
        var clinicStockMedicineDTO = mapper.Map<ClinicStockMedicineDTO>(clinicStockMedicine);
        return Ok(clinicStockMedicineDTO);
    }

    [HttpGet("Clinic/{clinicId}/Medicine/{medicineId}")]
    public async Task<ActionResult<ClinicStockMedicineDTO>> GetClinicStockMedicineAsync(int clinicId, int medicineId)
    {
        var clinicStockMedicine = await clinicStockMedicineService.GetClinicStockMedicineAsync(clinicId, medicineId);
        if (clinicStockMedicine == null)
        {
            return NotFound();
        }
        var clinicStockMedicineDTO = mapper.Map<ClinicStockMedicineDTO>(clinicStockMedicine);
        return Ok(clinicStockMedicineDTO);
    }
}
