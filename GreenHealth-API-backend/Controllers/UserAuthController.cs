using GreenHealth_API_backend.Models;
using GreenHealth_API_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User userParam)
        {
            var response = _userService.Authenticate(userParam.Email, userParam.Password);
            if (response.user == null)
                return BadRequest(new { message = "Email or password is incorrect" });
            return Ok(new { user = response.user, token = response.token });
        }

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] User user)
		{
			if (user == null) return StatusCode(StatusCodes.Status400BadRequest);

			user.IsAdmin = false;
			user.IsOwner = false;
			user.OrganisationId = 3;

			string userPass = user.Password;

			var newUser = await _userService.PostUser(user);
			if (newUser == null) return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

			var response = _userService.Authenticate(newUser.Email, userPass);
			if (response.user == null) return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

			return Ok(new { user = response.user, token = response.token });
		}
    }
}
