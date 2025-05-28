using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController(IMapper mapper, ISupplyService supplyService, IValidator<Medicine> medicineValidator, IValidator<Supply> supplyValidator) : ControllerBase
    {
        // GET: api/<SupplyController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplyDTO>>> GetSupplies()
        {
            var supplies = await supplyService.GetSuppliesAsync();
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
        public async Task<ActionResult<SupplyDTO>> AddToSupply([FromBody] Medicine medicine, int amount, int clinicId)
        {
            var resultMedicineValidation = medicineValidator.Validate(medicine);
            if (!resultMedicineValidation.IsValid)
            {
                return BadRequest(resultMedicineValidation.Errors);
            }
            var supply = await supplyService.AddToSupply(medicine, amount, clinicId);
            var resultSupplyValidation = supplyValidator.Validate(supply);
            if (!resultSupplyValidation.IsValid)
            {
                return BadRequest(resultSupplyValidation.Errors);
            }
            var supplyDTO = mapper.Map<SupplyDTO>(supply);
            return Ok(supplyDTO);
        }


        [HttpPost]
        public async Task<ActionResult<SupplyDTO>> AddSupply([FromBody] List<SupplyDTO> supplyDTOs)
        {
            var supplies = mapper.Map<List<Supply>>(supplyDTOs);
            var addedSupplies = await supplyService.AddSupplyAsync(supplies);
            var supplyDTOsResult = mapper.Map<List<SupplyDTO>>(addedSupplies);
            if (supplyDTOsResult == null || !supplyDTOsResult.Any())
            {
                return BadRequest("No supplies were added.");
            }
            return CreatedAtAction(nameof(GetSupplies), supplyDTOsResult);
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
}
