namespace MAS.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
