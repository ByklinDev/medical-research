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
public class ClinicStockMedicinesController(IMapper mapper, IClinicStockMedicineService clinicStockMedicineService) : ControllerBase
{
    // GET: api/<ClinicStockMedicineController>
    [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<ClinicStockMedicine>))]
    [PageListFilter<ClinicStockMedicineDTO>]
    public async Task<ActionResult<IEnumerable<ClinicStockMedicineDTO>>> GetClinicStockMedicines([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var clinicStockMedicines = await clinicStockMedicineService.GetClinicStockMedicinesAsync(query);
        var clinicStockMedicineDTOs = mapper.Map<List<ClinicStockMedicineDTO>>(clinicStockMedicines);
        var pagedDTO = new PagedList<ClinicStockMedicineDTO>(clinicStockMedicineDTOs, clinicStockMedicines.TotalCount, clinicStockMedicines.CurrentPage, clinicStockMedicines.PageSize);
        return Ok(pagedDTO);
    }

    // GET api/<ClinicStockMedicineController>/5
    [HttpGet("Clinics/{clinicId}")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<ClinicStockMedicine>))]
    [PageListFilter<ClinicStockMedicineDTO>]
    public async Task<ActionResult<IEnumerable<ClinicStockMedicineDTO>>> GetClinicStockMedicinesByClinicIdAsync(int clinicId, [FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var clinicStockMedicines = await clinicStockMedicineService.GetClinicStockMedicinesByClinicIdAsync(clinicId, query);
        var clinicStockMedicineDTOs = mapper.Map<List<ClinicStockMedicineDTO>>(clinicStockMedicines);
        var pagedDTO = new PagedList<ClinicStockMedicineDTO>(clinicStockMedicineDTOs, clinicStockMedicines.TotalCount, clinicStockMedicines.CurrentPage, clinicStockMedicines.PageSize);
        return Ok(pagedDTO);
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

    [HttpGet("Clinics/{clinicId}/Medicines/{medicineId}")]
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
