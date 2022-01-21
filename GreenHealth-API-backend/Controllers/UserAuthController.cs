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
    }
}
