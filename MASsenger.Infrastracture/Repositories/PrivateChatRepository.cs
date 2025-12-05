using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class PrivateChatRepository : BaseRepository<PrivateChat>, IPrivateChatRepository
    {
        public PrivateChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {
            
        }

        public async Task<IEnumerable<PrivateChat>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseChats WHERE Type == 'Private'";
            return (await _dapperDbContext.GetConnection().QueryAsync<PrivateChat>(query)).ToList();
        }

        public async Task<IEnumerable<PrivateChat>> GetAllUserAsync(Int32 userId)
        {
            return await _efDbContext.PrivateChats.Where(p => p.Starter.Id == userId && p.IsDeleted == false).ToListAsync();
        }
    }
}
