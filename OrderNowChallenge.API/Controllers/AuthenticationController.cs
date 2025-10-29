using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderNowChallenge.API.DTOs.Login;
using OrderNowChallenge.API.Helpers;
using OrderNowChallenge.Service.Users;

namespace OrderNowChallenge.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;

        public AuthenticationController(IUserService userServices, JwtHelper jwtHelper)
        {
            _userService = userServices;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(LoginRequest request)
        {
            var user = await _userService.Authenticate(request.UserName, request.Password);

            var token = _jwtHelper.GenerateJwtToken(user);

            return Ok(new LoginResponse(token));
        }
    }
}
