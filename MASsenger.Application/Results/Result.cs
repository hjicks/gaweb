using MASsenger.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace MASsenger.Application.Results
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

        public static Result Success(int statusCode) =>
            new(statusCode, true);

        public static SuccessResult<TResponse> Success<TResponse>(int statusCode, TResponse response) =>
            new(statusCode, true, response);

        public static FailureResult Failure(int statusCode, ErrorType error, IEnumerable<string> description) =>
            new(statusCode, false, error, description);

        public static FailureResult Failure(ErrorType error, IEnumerable<string> description) =>
            new(false, error, description);
    }

    public record SuccessResult<TResponse> : Result
    {
        [JsonPropertyOrder(1)]
        public TResponse Response { get; set; } = default!;

        public SuccessResult(int statusCode, bool ok, TResponse response)
            : base(statusCode, ok)
        {
            Response = response;
        }
    }

    public record FailureResult : Result
    {
        [JsonPropertyOrder(1)]
        public ErrorType Error { get; set; }

        [JsonPropertyOrder(2)]
        public IEnumerable<string> Description { get; set; } = null!;

        public FailureResult(int statusCode, bool ok, ErrorType error, IEnumerable<string> description)
            : base(statusCode, ok)
        {
            Error = error;
            Description = description;
        }

        public FailureResult(bool ok, ErrorType error, IEnumerable<string> description)
            : base(ok)
        {
            Error = error; 
            Description = description;
        }
    }
}
