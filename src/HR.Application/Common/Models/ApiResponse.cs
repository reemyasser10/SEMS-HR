namespace HR.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public string? ErrorCode { get; set; }
    public string? TraceId { get; set; }

    public ApiResponse()
    {
    }

    public static ApiResponse<T> SuccessResult(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> FailureResult(
        string message,
        IDictionary<string, string[]>? errors = null,
        string? errorCode = null,
        string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Errors = errors,
            ErrorCode = errorCode,
            TraceId = traceId
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResult(string? message = null)
    {
        return new ApiResponse
        {
            Success = true,
            Data = null,
            Message = message
        };
    }

    public static new ApiResponse FailureResult(
        string message,
        IDictionary<string, string[]>? errors = null,
        string? errorCode = null,
        string? traceId = null)
    {
        return new ApiResponse
        {
            Success = false,
            Data = null,
            Message = message,
            Errors = errors,
            ErrorCode = errorCode,
            TraceId = traceId
        };
    }
}
