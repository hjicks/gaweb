using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace MASsenger.Application.Responses
{
    public record Result
    {
        [JsonIgnore]
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;

        [JsonPropertyOrder(0)]
        public bool Ok { get; set; } = false;

        public Result(int statusCode, bool ok)
        {
            StatusCode = statusCode;
            Ok = ok;
        }

        public Result(bool ok)
        {
            Ok = ok;
        }

        public static SuccessResult<TResponse> Success<TResponse>(int statusCode, TResponse response) =>
            new(statusCode, true, response);

        public static FailureResult Failure(int statusCode, string error) =>
            new(statusCode, false, error);

        public static FailureResult Failure(string error) =>
            new(false, error);
    }

    public record SuccessResult<TResponse> : Result
    {
        [JsonPropertyOrder(1)]
        public TResponse Response { get; set; } = default!;

        public SuccessResult(int statusCode, bool ok, TResponse response) : base(statusCode, ok)
        {
            Response = response;
        }
    }

    public record FailureResult : Result
    {
        [JsonPropertyOrder(1)]
        public string Error { get; set; } = null!;

        public FailureResult(int statusCode, bool ok, string error) : base(statusCode, ok)
        {
            Error = error;
        }

        public FailureResult(bool ok, string error) : base(ok)
        {
            Error = error;
        }
    }
}
