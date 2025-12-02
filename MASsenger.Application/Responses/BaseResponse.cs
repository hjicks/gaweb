namespace MASsenger.Application.Responses
{
    public record BaseResponse
    {
        public string Description { get; set; } = null!;

        public BaseResponse()
        {

        }
        public BaseResponse(string description)
        {
            Description = description;
        }
    }
}
