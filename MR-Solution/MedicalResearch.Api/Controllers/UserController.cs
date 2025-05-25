using AutoMapper;
using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Enums;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace MedicalResearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IRoleService roleService, IMapper mapper, IValidator<UserCreateDTO> userValidator) : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await userService.GetUsersAsync();
            return  Ok(mapper.Map<List<UserDTO>>(users));
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
                return BadRequest(validationResult.Errors[0].ToString());
            }
            var user = mapper.Map<User>(userDTO);
            user.State = UserState.Active;            
            var createdUser = await userService.AddUserAsync(user);
            if (createdUser == null)
            {
                return BadRequest("User not created");
            }
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, mapper.Map<UserDTO>(createdUser));
        }


        [HttpPut]
        public async Task<ActionResult> AddUserRole([FromBody] int userId, int roleId)
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

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> EditUser(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest("User ID mismatch");
            }           
            var user = mapper.Map<User>(userDTO);
            var updatedUser = await userService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<UserDTO>(updatedUser));
        }


        [HttpPut]
        public async Task<ActionResult<UserState>> SetUserState([FromBody] int userId, UserState state)
        {
            var user = await userService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound($"User Id = {userId}");
            }
            if (user.State == state)
            {
                return BadRequest($"User Id = {userId} already has this state");
            }
            var result = await userService.SetState(user, state);
            if (user.State == result)
            {
                return BadRequest("User state not set");
            }
            return Ok(result);
        }

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

        [HttpDelete]
        public async Task<ActionResult> DeleteUserRole([FromBody] int userId, int roleId)
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
}
