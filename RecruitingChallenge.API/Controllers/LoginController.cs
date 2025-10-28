using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitingChallenge.API.DTOs.Login;
using RecruitingChallenge.API.Helpers;
using RecruitingChallenge.Service.Users;

namespace RecruitingChallenge.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;

        public LoginController(IUserService userServices, JwtHelper jwtHelper)
        {
            _userService = userServices;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userService.Authenticate(request.UserName, request.Password);

            if (user == null)
                throw new Exception("//to do - custom exception");

            var token = _jwtHelper.GenerateJwtToken(user);

            return Ok(new LoginResponse(token));
        }
    }
}
