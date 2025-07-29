using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Api.Filters;
using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalResearch.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, IRoleService roleService, IMapper mapper, IValidator<UserCreateDTO> userValidator, IValidator<UserUpdateDTO> updateValidator) : ControllerBase
{
    [Authorize]
    // GET: api/<UserController>
     [HttpGet]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<User>))]
    [PageListFilter<UserDTO>]
    public async Task<ActionResult<List<UserDTO>>> GetUsers([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var users = await userService.GetUsersAsync(query);
        var userDTOs = mapper.Map<List<UserDTO>>(users);
        var pagedDTO = new PagedList<UserDTO>(userDTOs, users.TotalCount, users.CurrentPage, users.PageSize);
        return Ok(pagedDTO);
    }


    [HttpGet("Roles")]
    [ServiceFilter(typeof(QueryDTOValidatorFilter<User>))]
    [PageListFilter<UserRoleDTO>]
    public async Task<ActionResult<List<UserRoleDTO>>> GetUsersRoles([FromQuery] QueryDTO queryDTO)
    {
        var query = mapper.Map<Query>(queryDTO);
        var users = await userService.GetUsersAsync(query);
        var userDTOs = mapper.Map<List<UserRoleDTO>>(users);
        var pagedDTO = new PagedList<UserRoleDTO>(userDTOs, users.TotalCount, users.CurrentPage, users.PageSize);
        return Ok(pagedDTO);
    }



    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<UserDTO>(user));
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserCreateDTO userDTO)
    {
        var validationResult = userValidator.Validate(userDTO);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors[0].ToString());
        }
        var user = mapper.Map<User>(userDTO);
        user.State = UserState.Active;
        var createdUser = await userService.AddUserAsync(user);
        if (createdUser == null)
        {
            throw new DomainException("User not created");
        }
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, mapper.Map<UserDTO>(createdUser));
    }

    [Authorize]
    [HttpPost("{userId}/Roles/{roleId}")]
    public async Task<ActionResult> AddUserRole(int userId, int roleId)
    {
        var user = await userService.GetUserAsync(userId);
        if (user == null)
        {
            return NotFound($"User Id = {userId}");
        }
        var role = await roleService.GetRoleAsync(roleId);
        if (role == null)
        {
            return NotFound($"Role Id = {roleId}");
        }
        var isAdded = await userService.AddUserRole(user, role);
        if (isAdded == false)
        {
            return BadRequest("User role not added");
        }
        return Ok();
    }



    [Authorize]
    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDTO>> EditUser(int id, [FromBody] UserUpdateDTO userDTO)
    {
        if (id != userDTO.Id)
        {
            throw new DomainException("User ID mismatch");
        }
        var validationResult = updateValidator.Validate(userDTO);
        if (!validationResult.IsValid)
        {
            throw new DomainException(validationResult.Errors[0].ToString());
        }

        var updatedUser = await userService.UpdateUserAsync(userDTO);
        if (updatedUser == null)
        {
            throw new DomainException("User wsa not updated");
        }

        return Ok(mapper.Map<UserDTO>(updatedUser));
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult<UserState>> SetUserState(int id, UserState state)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound($"User Id = {id}");
        }
        if (user.State == state)
        {
            return BadRequest($"User Id = {id} already has this state");
        }
        var result = await userService.SetState(user, state);
        if (user.State == result)
        {
            return BadRequest("User state not set");
        }
        return Ok(result);
    }



    [Authorize]
    [HttpPatch("{id}/photo")]
    public async Task<ActionResult<string>> UploadUserImage(int id, IFormFile file)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound($"User Id = {id}");
        }
        if (file.Length > 0)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var imageData = stream.ToArray();
                var result = await userService.SetImage(user, imageData);
                return Ok(result);
            }
        }
        return BadRequest("File is empty or not provided");
    }


    [Authorize]
    [HttpGet("{id}/photo")]
    public async Task<ActionResult<string>> GetUserImage(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound($"User Id = {id}");
        }

        var result = await userService.GetUserImage(user.Id);

        return Ok(result);
    }


    [Authorize]
    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var isDeleted = await userService.DeleteUserAsync(id);
        if (!isDeleted)
        {
            return BadRequest("User not deleted");
        }
        return Ok(isDeleted);
    }
    [Authorize]
    [HttpDelete("{userId}/Roles/{roleId}")]
    public async Task<ActionResult> DeleteUserRole(int userId, int roleId)
    {
        var user = await userService.GetUserAsync(userId);
        if (user == null)
        {
            return NotFound($"User Id = {userId}");
        }
        var role = await roleService.GetRoleAsync(roleId);
        if (role == null)
        {
            return NotFound($"Role Id = {roleId}");
        }
        var isDeleted = await userService.DeleteUserRole(user, role);
        if (isDeleted == false)
        {
            return BadRequest("User role not deleted");
        }
        return Ok();
    }

}
