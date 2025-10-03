using System;

namespace Infrastructure.Response;

public class Response<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public static Response<T> Success(T? data, string message)
    {
        return new Response<T>()
        {
            Data = data,
            IsSuccess = true,
            StatusCode = 201,
            Message = message
        };
    }
    public static Response<T> Fail(int statusCode,string message)
    {
        return new Response<T>()
        {
            Data = default,
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message
        };
    }
    public static Response<T> Done(string message){
        return new Response<T>()
        {
            Data = default,
            IsSuccess = true,
            StatusCode = 200,
            Message = message
        };
    }
}
