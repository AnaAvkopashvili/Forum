using Forum.Application.Accounts.Requests;
using Forum.Application.Accounts.Responses;
using Forum.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers.V1
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
            _logger.LogInformation("controller is executed");
        }

        [HttpGet]
        public async Task<List<AdminResponseModel>> GetAll()
        {
            return await _adminService.GetAllAsync();
        }

        [HttpGet("{userId}")]
        public async Task<List<ManageUserRolesResponseModel>> Get(string userId)
        {
            return await _adminService.GetAsync(userId);
        }

        [HttpPut("{userId}")]
        public async Task ManageUserRoles(string userId, [FromBody] List<ManageUserRolesRequestModel> model)
        {
            await _adminService.ManageUserRolesAsync(model, userId);
        }
    }
}