namespace MASsenger.Application.Responses
{
    public record GetEntityResponse<TEntity>

    {
        public IEnumerable<TEntity> Entities { get; set; } = null!; 

        public GetEntityResponse(IEnumerable<TEntity> entities) 
        {
            Entities = entities;
        }
    }
}
