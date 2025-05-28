using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController(IMapper mapper, IMedicineService medicineService) : ControllerBase
    {
        // GET: api/<MedicineController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetMedicines()
        {
            var medicines = await medicineService.GetMedicinesAsync();
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
}
