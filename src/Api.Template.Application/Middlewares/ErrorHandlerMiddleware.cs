using Serilog;
using System.Net;
using Microsoft.AspNetCore.Http;
using Api.Template.ViewModel.Common.Exception;
using Api.Template.ViewModel.Common.Response;

namespace Api.Template.Application.Middlewares;

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
        object result; string messages;
        
        switch (exception)
        {
            case ServiceException _exception:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    if (_exception.Errors != null)
                    {
                        result = new ErrorResponseModel()
                        {
                            Succeed = false,
                            Errors = _exception.Errors,
                            Message = _exception.Message,
                        };

                        break;
                    }
                    
                    result = new ResultResponseModel()
                    {
                        Succeed = false,
                        Message = _exception.Message,
                    };

                    break;
                }
            case NotFoundException _exception:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    messages = _exception.Message;
                    result = new ResultResponseModel()
                    {
                        Succeed = false,
                        Message = messages,
                    };

                    Log.Error(messages);
                    break;
                }
            default:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    messages = exception.Message + "\n" + (exception.InnerException != null ? exception.InnerException.Message : "") + "\n ***Trace*** \n" + exception.StackTrace;
                    result = messages;
                    
                    Log.Error(messages);
                    break;
                }
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result.ToString());
    }
}