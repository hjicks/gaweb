namespace MASsenger.Application.Responses
{
    public record GetEntityResponse<TEntity> : BaseResponse

    {
        public IEnumerable<TEntity> Entities { get; set; } = Enumerable.Empty<TEntity>(); 
    }
}
