using Microsoft.AspNetCore.Mvc;
using Forum.Application.Accounts;
using Forum.Application.Accounts.Requests;
using Forum.Application.Comments.Requests;
using Microsoft.AspNetCore.Authorization;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Forum.Persistence.Identity;
using k8s.KubeConfigModels;
using Forum.Web.Models;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public AccountController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.LoginAsync(model.Email, model.Password);
            if (result.Succeeded)
            {
                if (result.IsAdmin)
                    return RedirectToAction("GetAllForAdmin", "Topic");
                else
                    return RedirectToAction("GetAll", "Topic");
            }
            if(result.ErrorMessage != null)
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            var model = new RegistrationRequestModel
            {
                ProfileImageUrl = "/uploads/profile-pictures/default.jpg"
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegistrationRequestModel model)
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file != null)
            {
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");
                Directory.CreateDirectory(uploadDirectory);
                model.ImageUpload = file;
                string uniqueFileName = _userService.ProcessUploadedFile(file);
                model.ProfileImageUrl = $"/uploads/profile-pictures/{uniqueFileName}";
                var filePath = Path.Combine(uploadDirectory, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            else
            {
                model.ProfileImageUrl = "/uploads/profile-pictures/default.jpg";
            }
            var result = await _userService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);

                    return View(model);
                }
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Create(int? topicId)
        {
            if (topicId == null)
            {
                return NotFound();
            }
            var model = new CommentRequestPostModel { TopicId = topicId.Value };
            return View(model);
        }

        public async Task<IActionResult> EditUser(CancellationToken cancellationToken, string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = await _userService.GetAsync(cancellationToken, userId);
            var model = new UserPutModel
            { 
                Id = userId ,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                UserName = user.UserName,
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Put(CancellationToken cancellationToken, UserPutModel model)
        {
            var errors = ModelState.Select(x => x.Value?.Errors)
             .Where(y => y.Count > 0).ToList();

            if (!ModelState.IsValid)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error[0].ErrorMessage);
                }
            }
            try
            {

                await _userService.UpdateAsync(cancellationToken, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("EditUser", model);
            }
            var UserId = _currentUserService.GetCurrentUserId();

            return RedirectToAction("Index", "Home", new {id = UserId});
        }
    }
}