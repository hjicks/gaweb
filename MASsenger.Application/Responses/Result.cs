using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Responses
{
    public class Result
    {
        public bool Ok { get; set; } = false;
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public string Error { get; set; } = null!;

        private object _response = null!;
        public TResponse Response<TResponse>()
        {
            return (TResponse)_response;
        }
        private void SetResponse<TResponse>(TResponse response)
        {
            _response = response!;
        }

        private Result(bool ok, int statusCode, string error, object response)
        {
            Ok = ok;
            StatusCode = statusCode;
            Error = error;
            SetResponse(response);
        }

        public static Result Success(int statusCode, object response) =>
            new(true, statusCode, null!, response);
        public static Result Failure(int statusCode, string error) =>
            new(false, statusCode, error, null!);
    }
}
