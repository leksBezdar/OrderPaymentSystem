using Microsoft.AspNetCore.Mvc;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.Extensions;
using System.Reflection;
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

        var attribute = exception.GetType().GetCustomAttribute<ProblemDetailsAttribute>();
        if (attribute != null)
        {
            ApplyAttributeConfiguration(problemDetails, attribute, exception);
        }
        else
        {
            ConfigureGenericProblem(problemDetails, exception, env);
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

    private static void ApplyAttributeConfiguration(
        ProblemDetails problem,
        ProblemDetailsAttribute attribute,
        Exception exception)
    {
        problem.Type = attribute.Type;
        problem.Title = attribute.Title;
        problem.Status = attribute.StatusCode;
        problem.Detail = exception.Message;

        if (attribute.IncludeEntityInfo && exception is IEntityException entityEx)
        {
            problem.Extensions["entity"] = new
            {
                entityEx.EntityName,
                entityEx.FieldName,
                entityEx.FieldValue
            };
        }
    }
}