using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class ChannelChatRepository : BaseRepository<ChannelChat>, IChannelChatRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public ChannelChatRepository(EfDbContext efContext, DapperDbContext dapperDbContext) : base(efContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<ChannelChat>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseChats WHERE Type == 'Channel'";
            return (await _dapperDbContext.GetConnection().QueryAsync<ChannelChat>(query)).ToList();
        }
    }
}
