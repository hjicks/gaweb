using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.MessageEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;

namespace MAS.Infrastracture.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {
            
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseMessages WHERE Type == 'Message'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Message>(query)).ToList();
        }
    }
}
