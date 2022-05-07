namespace Api.Template.Application.Middlewares;

using Serilog;
using System.Net;
using Microsoft.AspNetCore.Http;
using ViewModel.Common;
using ViewModel.Common.Exception;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        object result;
        string errorMessages;
        switch (exception)
        {
            case ServiceException _exception:
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorMessages = _exception.Message;
                result = new ResultModel()
                {
                    Succeed = false,
                    ErrorMessages = errorMessages,
                };

                break;
            }
            case NotFoundException _exception:
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                errorMessages = _exception.Message;
                result = new ResultModel()
                {
                    Succeed = false,
                    ErrorMessages = errorMessages,
                };

                Log.Error(errorMessages);
                break;
            }
            default:
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorMessages = exception.Message + "\n" +
                                (exception.InnerException != null ? exception.InnerException.Message : "") +
                                "\n ***Trace*** \n" + exception.StackTrace;
                result = new ResultModel()
                {
                    Succeed = false,
                    ErrorMessages = errorMessages,
                };

                Log.Error(errorMessages);
                break;
            }
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result.ToString());
    }
}