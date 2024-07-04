using Cyberbit.TaskManager.Server.AuthorizeHelpers;
using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Cyberbit.TaskManager.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersBl _usersBl;
        private readonly IAutoMapperService _autoMapper;

        public UsersController(ILogger<UsersController> logger, IUsersBl usersBl, IAutoMapperService autoMapper)
        {
            _logger = logger;
            _usersBl = usersBl;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var allUsers = await _usersBl.GetAllUser();
            if (allUsers == null)
            {
                var errMsg = "Failed to get all users";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }
            var allUsersDto = _autoMapper.Mapper.Map<List<UserDto>>(allUsers);
            return allUsersDto;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int id)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to get user with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var user = await _usersBl.GetUserById(id);
            if (user == null)
            {
                var errMsg = $"Failed to get user with id '{id}. User not found'";
                _logger.LogError(errMsg);
                return NotFound(errMsg);
            }
            var UserDto = _autoMapper.Mapper.Map<UserDto>(user);
            return UserDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                var errMsg = $"Input user dto model is invalid";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var user = _autoMapper.Mapper.Map<User>(userDto);
            var addedUser = await _usersBl.AddUser(user);
            if (addedUser == null)
            {
                var errMsg = $"Failed to add user. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var addedUserDto = _autoMapper.Mapper.Map<UserDto>(addedUser);
            return addedUserDto;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id, [FromBody] UserDto userDto)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to update user with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            if (id != userDto.Id)
            {
                var errMsg = $"Failed to update user. Id in url and in dto are mistmatched";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var user = _autoMapper.Mapper.Map<User>(userDto);
            User updatedUser = null;
            try
            {
                updatedUser = await _usersBl.UpdateUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to updated user '{id}'");
            }

            if (updatedUser == null)
            {
                var errMsg = $"Failed to update user with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var updatedUserDto = _autoMapper.Mapper.Map<UserDto>(updatedUser);
            return updatedUserDto;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> DeleteUserById([FromRoute] int id)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to delete user with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            User user = null;
            try
            {
                user = await _usersBl.DeleteUserById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to delete user '{id}'");
            }

            if (user == null)
            {
                var errMsg = $"Failed to delete user with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var deletedUserDto = _autoMapper.Mapper.Map<UserDto>(user);
            return deletedUserDto;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticateRequest request)
        {
            var msg = "Username or password is incorrect";
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogError(msg);
                return BadRequest(new { message = msg });
            }

            var response = await _usersBl.Authenticate(request);
            if (response == null)
            {
                _logger.LogError(msg);
                return BadRequest(new { message = msg });
            }

            if (string.IsNullOrEmpty(response.Token))
            {
                var notAllowedMsg = "The user is not allowed to Log in to the system ";
                _logger.LogError(notAllowedMsg);
                return BadRequest(new { message = notAllowedMsg });
            }

            return response;
        }
    }
}
