using Domain.Exceptions;
using Domain.Shared;
using Domain.Shared.Constants;
using Domain.Shared.ProblemDetails;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

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
		var statusCodeAsInt = (int)statusCode;

		context.Response.ContentType = MediaTypeNames.Application.Json;
		context.Response.StatusCode = statusCodeAsInt;

		var problemDetails = GetProblemDetails(context, exception, statusCode, statusCodeAsInt);

		var json = JsonConvert.SerializeObject(
			problemDetails,
			new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});

		return context.Response.WriteAsync(json);
	}

	private static ProblemDetails GetProblemDetails(
		HttpContext context,
		Exception exception,
		HttpStatusCode statusCode,
		int statusCodeAsInt)
	{
		var rfcType = GetRFCType(statusCode);
		var errors = exception is ValidationException validationException
			? validationException
				.Errors
				.GroupBy(v => v.PropertyName)
				.Select(g =>
					new Error
					{
						Property = g.Key,
						ErrorMessages = g.Select(v => v.ErrorMessage).Distinct()
					})
			: null;

		return new(
			rfcType,
			exception.Message,
			statusCodeAsInt,
			context.Request.Path,
			exception.Message,
			errors);
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