using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;

namespace MAS.Infrastracture.Repositories
{
    public class ChannelChatRepository : BaseRepository<ChannelChat>, IChannelChatRepository
    {
        public ChannelChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {

        }

        public async Task<IEnumerable<ChannelChat>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseChats WHERE Type == 'Channel'";
            return (await _dapperDbContext.GetConnection().QueryAsync<ChannelChat>(query)).ToList();
        }
    }
}
