using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories
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
