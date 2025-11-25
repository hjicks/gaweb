using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public MessageRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseMessages WHERE Type == 'Message'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Message>(query)).ToList();
        }
    }
}
