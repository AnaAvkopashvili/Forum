using Forum.Application.Accounts.Requests;
using Forum.Application.Accounts.Responses;
using Forum.Application.Comments.Responses;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Forum.Application.Accounts
{
    public interface IUserService
    {
        Task<LoginResult> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<UserPutModel> GetByEmailAsync(CancellationToken cancellationToken, string email);
        Task<IdentityResult> RegisterAsync(RegistrationRequestModel model);
        Task UpdateAsync(CancellationToken cancellationToken, UserPutModel model);
        Task<UserPutModel> GetAsync(CancellationToken cancellationToken, string id);
        string ProcessUploadedFile(IFormFile file);
    }
}
