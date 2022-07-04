namespace Api.Template.ViewModel.Common.Exception;

public class ServiceException: System.Exception
{
    public dynamic Errors { get; set; }
        
    public ServiceException() : base() { }

    public ServiceException(string message) : base(message) { }

    public ServiceException(string message, dynamic errors) : base(message)
    {
        Errors = errors;
    }
}