using Forum.Application.Accounts.Requests;
using Forum.Application.Accounts.Responses;
using Forum.Application.Exceptions;
using Forum.Application.Topics;
using Forum.Application.Topics.Responses;
using Forum.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Accounts
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _repository;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
        }

        public async Task<IdentityResult> RegisterAsync(RegistrationRequestModel model)
        {
            var UserByEmail = await _userManager.FindByEmailAsync(model.Email);
            var UserByUserName = await _userManager.FindByNameAsync(model.UserName);
            if (UserByEmail != null || UserByUserName != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "This email or Username is already registered." });
            }
            var user = model.Adapt<User>();
            await _userManager.AddToRoleAsync(user, "User");

            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new LoginResult { Succeeded = false };
            }

            if (user.BanExpiration != null)
            {
                return new LoginResult { Succeeded = false, ErrorMessage = "Your account is currently banned." };
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (signInResult.Succeeded)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                return new LoginResult { Succeeded = true, User = user, IsAdmin = isAdmin };
            }

            return new LoginResult { Succeeded = false, ErrorMessage = "Your Email or Password is incorrect." };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserPutModel> GetAsync(CancellationToken cancellationToken, string id)
        {
            var result = await _repository.GetAsync(cancellationToken, id);
            if (result == null)
            {
                throw new UserNotFound("User with this ID: " + id.ToString() + " was not found!");
            }
            return result.Adapt<UserPutModel>();
        }

        public async Task<UserPutModel> GetByEmailAsync(CancellationToken cancellationToken, string email)
        {
            var allUsers = await _repository.GetAllAsync(cancellationToken);
            var user = allUsers.SingleOrDefault(x => x.Email == email);    
            if(user == null)
            {
                throw new UserNotFound("User with this Email: " + email.ToString() + " was not found!");
            }
            return user.Adapt<UserPutModel>();
        }

        public async Task UpdateAsync(CancellationToken cancellationToken, UserPutModel model)
        {
            if (!await _repository.Exists(cancellationToken, model.Id))
            {
                throw new UserNotFound("User with this ID: " + model.Id.ToString() + " was not found!");
            }

            var existingUser = await _userManager.FindByIdAsync(model.Id.ToString());
            if (existingUser == null)
            {
                throw new UserNotFound("User with this ID: " + model.Id.ToString() + " was not found!");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, model.OldPassword);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccess("Incorrect password. Please confirm your identity!");
            }
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var result = await _userManager.RemovePasswordAsync(existingUser);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(existingUser, model.NewPassword);
                }
            }
           
            existingUser.FirstName = model.FirstName;
            existingUser.LastName = model.LastName;
            existingUser.Gender = model.Gender; 
            existingUser.Email = model.Email;
            existingUser.UserName = model.UserName;
            var UserByUserName = await _userManager.FindByNameAsync(model.UserName);

            if (UserByUserName != null && existingUser.UserName != model.UserName)
            {
                throw new UserAlreadyExists("Can't update! User with this Emal or Username already exists!");
            }
            else
            {
                await _repository.UpdateAsync(cancellationToken, existingUser);
            }
        }

        public string ProcessUploadedFile(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            return fileName;
        }
    }
    
        public class LoginResult    
        {
            public bool Succeeded { get; set; }
            public User User { get; set; }
            public bool IsAdmin { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
