using Forum.API.Infrastructure.Middlewares;

namespace Forum.API.Infrastructure.Extensions
{
    public static class RequestResponseLoggingExtention
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestResponseLoggingMiddleware>();
            return builder;
        }
    }
}
