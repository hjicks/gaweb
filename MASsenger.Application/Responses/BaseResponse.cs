namespace MASsenger.Application.Responses
{
    public record BaseResponse
    {
        public string Message { get; set; } = null!;

        public BaseResponse()
        {

        }
        public BaseResponse(string message)
        {
            Message = message;
        }
    }
}
