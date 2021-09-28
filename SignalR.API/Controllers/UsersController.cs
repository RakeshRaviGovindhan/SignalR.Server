using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.Model.DbModel;
using SignalR.Service.Core;
using SignalR.Service.Extension;
using System;

namespace SignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Insert")]
        public IActionResult InserUser([FromBody] UserModel userModel)
        {
            try
            {
                var response = userService.InsertUser(userModel);
                if (userModel.Id != Guid.Empty)
                {
                    var user = userService.GetUserById(userModel.Id);
                }
                return Ok(response);
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var userModel = userService.Login(loginModel, HttpContext);

                if (userModel == null || !string.IsNullOrEmpty(userModel.ValidationToken))
                {
                    return BadRequest(new { message = "Login or password is incorrect" });
                }
                return Ok(userModel.WithoutPassword());
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var userModel = userService.RefreshToken(refreshToken, HttpContext);
                if (userModel == null)
                {
                    // TODO Log Error Refresh Token not found.
                    return Unauthorized();
                }
                return Ok(userModel);
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }

        [HttpGet("getMyFriends/{userId}")]
        public IActionResult GetMyFriends(Guid userId)
        {
            try
            {
                return Ok(userService.GetMyFriends(userId));
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
    }
}
