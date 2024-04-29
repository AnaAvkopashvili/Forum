using Forum.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Forum.API.Infrastructure
{
    public class GlobalExceptionHandler : ProblemDetails
    {
        public const string UnhandlerErrorCode = "UnhandledError";
        private HttpContext _httpContext;
        private Exception _exception;

        public LogLevel LogLevel { get; set; }
        public string Code { get; set; }

        public string? TraceId
        {
            get
            {
                if (Extensions.TryGetValue("TraceId", out var traceId))
                {
                    return (string?)traceId;
                }
                return null;
            }
            set => Extensions["TraceId"] = value;
        }

        public GlobalExceptionHandler(HttpContext httpContext, Exception exception)
        {
            _httpContext = httpContext;
            _exception = exception;

            TraceId = httpContext.TraceIdentifier;

            Code = UnhandlerErrorCode;
            Status = (int)HttpStatusCode.InternalServerError;
            Title = "Internal Server Error";
            LogLevel = LogLevel.Error;
            Instance = httpContext.Request.Path;

            HandleException((dynamic)exception);
        }

        private void HandleException(TopicNotFound exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(UserNotFound exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(CommentNotFound exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(UnauthorizedAccess exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Unauthorized;
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(InvalidOperation exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.BadRequest;
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(Gone exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Gone;
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(UserAlreadyExists exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Conflict;
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
        private void HandleException(Exception exception)
        {

        }
    }
}