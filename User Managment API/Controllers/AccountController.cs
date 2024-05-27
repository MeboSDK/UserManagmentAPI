using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _userService.RegisterAsync(userDto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            var token = await _userService.LoginAsync(email, password);
            return Ok(new { Token = token });
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var userProfile = await _userService.GetUserProfileAsync(userId);
            return Ok(userProfile);
        }

        [HttpPut("profile")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserDTO userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await _userService.UpdateUserProfileAsync(userId, userDto);
            if (success) return NoContent();

            return BadRequest("Failed to update user profile");
        }
    }
}
