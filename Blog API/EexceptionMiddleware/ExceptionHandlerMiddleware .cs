using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Net;
using System.Text.Json;

namespace Blog_API.EexceptionMiddleware
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode code;
            switch (exception)
            {
                case KeyNotFoundException
                    or NotFoundException
                    or FileNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException
                    or UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case BadRequestException
                    or ArgumentException
                    or InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }
            return (code, JsonSerializer.Serialize(new {error= exception.Message, statusCode=code }));
        }
    }
}

