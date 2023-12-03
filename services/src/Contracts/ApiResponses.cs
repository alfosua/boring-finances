using System.Text.Json.Serialization;

namespace BoringFinances.Services.Contracts;

public interface IApiResponse
{
    public bool Ok { get; }
}

public class BasicResponse : IApiResponse
{
    public BasicResponse() { }
    public BasicResponse(bool ok = true) => Ok = ok;
    public bool Ok { get; set; } = true;
}

public class SingleResponse<T> : IApiResponse
{
    public SingleResponse() { }
    public SingleResponse(bool ok = true) => Ok = ok;
    public bool Ok { get; set; } = true;
    public required T Data { get; set; }
}

public class ArrayResponse<T> : IApiResponse
{
    public ArrayResponse() { }
    public ArrayResponse(bool ok = true) => Ok = ok;
    public bool Ok { get; set; } = true;
    public required ICollection<T> Data { get; set; }
    public int Count => Data.Count;
}

public class ErrorResponse : IApiResponse
{
    public ErrorResponse() { }
    public ErrorResponse(bool ok = false) => Ok = ok;
    public bool Ok { get; set; } = false;
    public ICollection<ErrorResponseItem> Errors { get; set; } = Array.Empty<ErrorResponseItem>();
}

public class ErrorResponseItem
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Exception { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTrace { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InnerMessage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InnerException { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InnerStackTrace { get; set; }
}

public static class ApiResponse
{
    public static BasicResponse CreateBasic(bool ok = true) => new BasicResponse(ok);

    public static SingleResponse<T> CreateSingle<T>(T data, bool ok = true) => new SingleResponse<T>(ok) { Data = data };

    public static ArrayResponse<T> CreateArray<T>(IEnumerable<T> data, bool ok = true) => new ArrayResponse<T>(ok) { Data = data.ToArray() };

    public static ErrorResponse CreateError(IEnumerable<ErrorResponseItem> errors, bool ok = false) => new ErrorResponse(ok) { Errors = errors.ToArray() };
}
