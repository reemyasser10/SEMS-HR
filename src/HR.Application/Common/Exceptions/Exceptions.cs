namespace HR.Application.Common.Exceptions;

public abstract class AppException : Exception
{
    public string ErrorCode { get; }

    protected AppException(string message, string errorCode = "APPLICATION_ERROR")
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message = "ResourceNotFound")
        : base(message, "NOT_FOUND")
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"{entityName}NotFound", "NOT_FOUND")
    {
    }
}

public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base(message, "BAD_REQUEST")
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "UnauthorizedAccess")
        : base(message, "UNAUTHORIZED")
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "ForbiddenAccess")
        : base(message, "FORBIDDEN")
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }
}

public class AppValidationException : AppException
{
    public IDictionary<string, string[]> Errors { get; }

    public AppValidationException(IDictionary<string, string[]> errors, string message = "ValidationError")
        : base(message, "VALIDATION_ERROR")
    {
        Errors = errors;
    }

    public AppValidationException(string field, string error, string message = "ValidationError")
        : base(message, "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { error } }
        };
    }
}
