using Domain.Exceptions;
using Domain.Shared.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = GetProblemDetails(context, exception, statusCode);
        var problemDetailsJson = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(problemDetailsJson);
    }

    private static ProblemDetails GetProblemDetails(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        var validationMessages = exception is ValidationException validationException
            ? validationException
                .Errors
                .GroupBy(v => v.PropertyName)
                .Select(g =>
                    new
                    {
                        Property = g.Key,
                        ValidationMessages = g.Select(v => v.ErrorMessage).Distinct()
                    })
            : null;

        var rfcType = GetRFCType(statusCode);

        var exceptionDetail = validationMessages is not null
            ? JsonSerializer.Serialize(validationMessages)
            : exception.Message;

        ProblemDetails problemDetails = new()
        {
            Type = rfcType,
            Status = (int)statusCode,
            Detail = exceptionDetail,
            Instance = context.Request.Path
        };

        return problemDetails;
    }

    private static HttpStatusCode GetHttpStatusCode(Exception exception) =>
        exception switch
        {
            NullReferenceException => HttpStatusCode.NotFound,
            NotEnoughRightsException => HttpStatusCode.Forbidden,
            IncorrectPasswordException
                or ValueNotUniqueException
                or IncorrectExtensionException
                or OperationFailedException
                or OperationCanceledException
                or ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

    private static string GetRFCType(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.NotFound => RFCType.NotFound,
            HttpStatusCode.BadRequest => RFCType.BadRequest,
            HttpStatusCode.Forbidden => RFCType.Forbidden,
            _ => RFCType.InternalServerError
        };
}