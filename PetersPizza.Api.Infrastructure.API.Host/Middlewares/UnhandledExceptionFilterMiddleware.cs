using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace PetersPizza.Api.Infrastructure.API.Host.Middlewares;

[ExcludeFromCodeCoverage]
public class UnhandledExceptionFilterMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var response = new ProblemDetails
                {
                    Title = "Validation failed",
                    Detail = validationException.Errors.First().ErrorMessage,
                    Status = (int)HttpStatusCode.BadRequest
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new ProblemDetails
                {
                    Title = "An unexpected error happened",
                    Detail = exception.Message,
                    Status = (int)HttpStatusCode.InternalServerError
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}