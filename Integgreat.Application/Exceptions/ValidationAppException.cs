namespace Integgreat.Application.Exceptions;

public class ValidationAppException : AppException
{
    public ValidationAppException(string message) : base(message, 400) { }
}
