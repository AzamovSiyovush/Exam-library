using System;
using Domain.Enums;

namespace Infrastructure.ApiResult;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public ErrorType ErrorType { get; set; }

    public static Result<T> Ok(T? data, string? message = null) => new()
    {
        IsSuccess = true,
        Message = message,
        Data = data
    };
    public static Result<T> Fail(string message, ErrorType errorType = ErrorType.Internal) => new()
    {
        IsSuccess = false,
        Message = message,
        ErrorType = errorType
    };
}
