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



namespace MedicalResearch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IMapper mapper, IRoleService roleService) : ControllerBase
{
    // GET: api/<RoleController>
    [HttpGet]
    [ServiceFilter(typeof(CheckDTOFilterAttribute<Role>))]
    [PageListFilter<RoleDTO>]
    public async Task<ActionResult<List<RoleDTO>>> GetRoles([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var roles = await roleService.GetRolesAsync(query);
        var rolesDTOs = mapper.Map<List<RoleDTO>>(roles);
        var pagedDTO = new PagedList<RoleDTO>(rolesDTOs, roles.TotalCount, roles.CurrentPage, roles.PageSize);
        return Ok(pagedDTO);        
    }
    

    // GET api/<RoleController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDTO>> Get(int id)
    {
        var role = await roleService.GetRoleAsync(id);
        if (role == null)
        {
            return NotFound(id);
        }
        return Ok(mapper.Map<RoleDTO>(role));
    }

    // POST api/<RoleController>
    [HttpPost]
    public async Task<ActionResult<RoleDTO>> AddRole([FromBody] RoleCreateDTO roleCreateDTO )
    {
        var role = mapper.Map<Role>(roleCreateDTO);
        var createdRole = await roleService.AddRoleAsync(role);
        if (createdRole == null)
        {
            return BadRequest("Role not created");
        }
        return CreatedAtAction(nameof(Get), new { id = createdRole.Id }, mapper.Map<RoleDTO>(createdRole));
    }

    // PUT api/<RoleController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDTO>> EditRole(int id, [FromBody] RoleDTO roleDTO)
    {
        if (id != roleDTO.Id)
        {
            return BadRequest("Id mismatch");
        }
        var role = mapper.Map<Role>(roleDTO);
        var updatedRole = await roleService.UpdateRoleAsync(role);
        if (updatedRole == null)
        {
            return NotFound(id);
        }
        return Ok(mapper.Map<RoleDTO>(updatedRole));
    }

    // DELETE api/<RoleController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteRole(int id)
    {
        var role = await roleService.GetRoleAsync(id);
        if (role == null)
        {
            return NotFound(id);
        }
        var isDeleted = await roleService.DeleteRoleAsync(id);
        if (isDeleted == false)
        {
            return BadRequest("Role not deleted");
        }
        return Ok(isDeleted);
    }
}
