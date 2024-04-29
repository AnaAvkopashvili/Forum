using Asp.Versioning;
using Forum.API.Infrastructure.JWT;
using Forum.Application.Accounts;
using Forum.Application.Accounts.Requests;
using Forum.Application.Exceptions;
using Forum.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Forum.API.Controllers.V1
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion(1)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IOptions<JWTConfiguration> _options;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserService userService, IOptions<JWTConfiguration> options, IAdminService adminService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _options = options;
            _adminService = adminService;
            _logger = logger;
            _logger.LogInformation("controller is executed");
        }

        [HttpPost("register")]
        public async Task Register(RegistrationRequestModel user)
        {
            var result = await _userService.RegisterAsync(user);
            if (!result.Succeeded)
            {
                throw new UserAlreadyExists("Registration failed, user with this Email or Username already exists!");
            }
        }

        [HttpPost("login")]
        public async Task<string> LogIn(LoginRequestModel user)
        {
            var result = await _userService.LoginAsync(user.Email, user.Password);
            if (result.Succeeded)
            {
                var roles = await _adminService.GetUserRolesAsync(user.Email);

                return JWTHelper.GenerateSecurityToken(user.Email, roles, _options);
            }
            throw new UnauthorizedAccess(result.ErrorMessage);

        }
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _userService.LogoutAsync();
        }
    }
}


namespace ApiVersioning.Controllers.V2
{
    [Route("[controller]")]
    [ApiVersion(2, Deprecated = true)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public string GetAll()
        {
            return "1";
        }
    }
}