using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace MASsenger.Infrastracture.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly EfDbContext _efContext;
        private readonly DapperDbContext _dapperDbContext;
        public SessionRepository(EfDbContext efContext, DapperDbContext dapperDbContext)
        {
            _efContext = efContext;
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            string query = "SELECT * FROM Sessions";
            return (await _dapperDbContext.GetConnection().QueryAsync<Session>(query)).ToList();
        }

        public async Task<Session> GetActiveSessionByUserIdAsync(Int32 userId)
        {
            // failed dapper approach. the problem is conversion of string to guid.
            // when dapper reads from db, it brings string but the type of session token is guid.

            //string query = $"SELECT * FROM Sessions WHERE UserId == {userId} AND IsExpired == false";
            //return await _dapperDbContext.GetConnection().QuerySingleAsync<Session>(query);

            return await _efContext.Sessions.Where(s => s.User.Id == userId && s.IsExpired == false).SingleAsync();
        }

        public async Task Add(Session session)
        {
            await _efContext.Sessions.AddAsync(session);
        }
    }
}
