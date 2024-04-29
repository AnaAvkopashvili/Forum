using FluentValidation.AspNetCore;
using FluentValidation;
using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Services;
using Forum.Application.Topics;
using Forum.Domain.Entities;
using Forum.Infrastructure.Comments;
using Forum.Infrastructure.Topics;
using Forum.Infrastructure.UOW;
using Forum.Infrastructure.Users;
using Forum.Persistence;
using Forum.Persistence.Identity;
using Forum.Persistence.Seed;
using Forum.Web.BackgroundServices;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Forum.Web
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentUOW, CommentUOW>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserManager<User>>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHostedService<UnbanUserBackgroundService>();
            services.AddHostedService<TopicArchivalBackgroundService>();
        }
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHealthChecks();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10);
                opt.MaximumHistoryEntriesPerEndpoint(60);
                opt.SetApiMaxActiveRequests(1);    
                opt.AddHealthCheckEndpoint("feedback api", "/api/health");  

            }).AddInMemoryStorage();

            ConfigureServices(builder.Services);

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddRazorPages();


            builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

                options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));

                options.AddPolicy("AdminAndUser", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||
                        context.User.IsInRole("User")));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Login";
                 options.Cookie.Name = "AuthCookie";
             });


            builder.Services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Register your services here
            var app = builder.Build();  

            app.UseHttpsRedirection();

            app.UseStaticFiles();
           // app.UseSession();
            app.UseRouting();
          
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/api/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = "/healthcheck-ui";
                //options.AddCustomStylesheet("./HealthCheck/Custom.css");

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            await DataSeeder.Initialize(app.Services);

            app.Run();

        }
        
    }
}
