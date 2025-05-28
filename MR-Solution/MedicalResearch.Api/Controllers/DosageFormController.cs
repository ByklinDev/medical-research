using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DosageFormController(IMapper mapper, IDosageFormService dosageFormService) : ControllerBase
    {
        // GET: api/<DosageFormController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DosageFormDTO>>> GetDosageForms()
        {
            var dosageForms = await dosageFormService.GetDosageFormsAsync();
            var dosageFormDTOs = mapper.Map<List<DosageFormDTO>>(dosageForms);
            return Ok(dosageFormDTOs);
        }

        // GET api/<DosageFormController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DosageFormDTO>> GetDosageForm(int id)
        {
            var dosageForm = await dosageFormService.GetDosageFormAsync(id);
            if (dosageForm == null)
            {
                return NotFound(id);
            }
            var dosageFormDTO = mapper.Map<DosageFormDTO>(dosageForm);
            return Ok(dosageFormDTO);
        }

        // POST api/<DosageFormController>
        [HttpPost]
        public async Task<ActionResult<DosageFormDTO>> AddDosageForm([FromBody] DosageFormCreateDTO dosageFormCreateDTO)
        {
            if (dosageFormCreateDTO == null)
            {
                return BadRequest("Dosage form data is null");
            }
            var dosageForm = mapper.Map<DosageForm>(dosageFormCreateDTO);
            var dosageFormAdded = await dosageFormService.AddDosageFormAsync(dosageForm);
            if (dosageFormAdded == null)
            {
                return BadRequest("Dosage form could not be added");
            }
            var dosageFormDTO = mapper.Map<DosageFormDTO>(dosageFormAdded);
            return CreatedAtAction(nameof(GetDosageForm), new { id = dosageForm.Id }, dosageFormDTO);
        }

        // PUT api/<DosageFormController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DosageFormDTO>> EditDosageForm (int id, [FromBody] DosageFormDTO dosageFormDTO)
        {
            if (dosageFormDTO == null || dosageFormDTO.Id != id)
            {
                return BadRequest("Id mismatch");
            }
            var dosageForm = mapper.Map<DosageForm>(dosageFormDTO);
            var updatedDosageForm = await dosageFormService.UpdateDosageFormAsync(dosageForm);
            if (updatedDosageForm == null)
            {
                return NotFound(id);
            }
            var updatedDosageFormDTO = mapper.Map<DosageFormDTO>(updatedDosageForm);
            return Ok(updatedDosageFormDTO);
        }

        // DELETE api/<DosageFormController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDosageForm(int id)
        {
            var deleted = await dosageFormService.DeleteDosageFormAsync(id);
            if (!deleted)
            {
                return BadRequest("Dosage form is not deleted");
            }
            return Ok(deleted);
        }
    }
}
