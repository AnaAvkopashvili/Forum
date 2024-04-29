using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Services;
using Forum.Application.Topics;
using Forum.Domain.Entities;
using Forum.Infrastructure.Comments;
using Forum.Infrastructure.Topics;
using Forum.Infrastructure.UOW;
using Forum.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;

namespace Forum.API.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ICommentUOW, CommentUOW>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

        }
    }
}
