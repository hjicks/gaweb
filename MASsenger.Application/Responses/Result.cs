using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace MASsenger.Application.Responses
{
    public class Result
    {
        [JsonIgnore]
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;

        [JsonPropertyOrder(0)]
        public bool Ok { get; set; } = false;

        [JsonPropertyOrder(1)]
        public string Error { get; set; } = null!;

        public Result(int statusCode, bool ok, string error)
        {
            StatusCode = statusCode;
            Ok = ok;
            Error = error;
        }

        public static Result<TResponse> Success<TResponse>(int statusCode, TResponse response) =>
            new(statusCode, true, null!, response);

        public static Result Failure(int statusCode, string error) =>
            new(statusCode, false, error);
    }

    public class Result<TResponse> : Result
    {
        [JsonPropertyOrder(2)]
        public TResponse Response { get; set; } = default!;
        public Result(int statusCode, bool ok, string error, TResponse response) : base(statusCode, ok, error)
        {
            Response = response;
        }
    }
}
