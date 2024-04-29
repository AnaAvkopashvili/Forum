using Forum.API.Infrastructure.Middlewares;

namespace Forum.API.Infrastructure.Extensions
{
    public static class GlobalExceptionHandlingExtentension
    {
        public static IApplicationBuilder UseExceptionhandling(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            return builder;
        }
    }
}
