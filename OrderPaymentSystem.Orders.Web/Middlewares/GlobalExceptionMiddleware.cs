using Microsoft.AspNetCore.Mvc;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.Extensions;
using System.Text.Json;

namespace OrderPaymentSystem.Orders.Web.Middlewares;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Global error: {ErrorText}", exception.ToText());

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path,
            Extensions = { ["traceId"] = context.TraceIdentifier }
        };

        switch (exception)
        {
            case DuplicateEntityException dex:
                ConfigureDuplicateEntityProblem(dex, problemDetails);
                break;

            case EntityNotFoundException nex:
                ConfigureNotFoundProblem(nex, problemDetails);
                break;

            default:
                ConfigureGenericProblem(problemDetails, exception, env);
                break;
        }

        if (env.IsDevelopment())
        {
            problemDetails.Extensions["debug"] = new
            {
                exception.StackTrace,
                InnerException = exception.InnerException?.Message
            };
        }

        context.Response.StatusCode = problemDetails.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsJsonAsync(problemDetails, options);
    }

    private static void ConfigureDuplicateEntityProblem(
        DuplicateEntityException ex,
        ProblemDetails problem)
    {
        problem.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
        problem.Title = "Conflict";
        problem.Status = StatusCodes.Status409Conflict;
        problem.Detail = ex.Message;
        problem.Extensions["entity"] = new
        {
            ex.EntityName,
            ex.FieldName,
            ex.FieldValue
        };
    }

    private static void ConfigureNotFoundProblem(
        EntityNotFoundException ex,
        ProblemDetails problem)
    {
        problem.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
        problem.Title = "Not Found";
        problem.Status = StatusCodes.Status404NotFound;
        problem.Detail = ex.Message;
        problem.Extensions["entity"] = new
        {
            ex.EntityName,
            ex.FieldName,
            ex.FieldValue
        };
    }

    private static void ConfigureGenericProblem(
        ProblemDetails problem,
        Exception exception,
        IHostEnvironment env)
    {
        problem.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        problem.Title = "Internal Server Error";
        problem.Status = StatusCodes.Status500InternalServerError;
        problem.Detail = env.IsDevelopment()
            ? exception.ToString()
            : "An unexpected error occurred. Please try again later.";
    }
}