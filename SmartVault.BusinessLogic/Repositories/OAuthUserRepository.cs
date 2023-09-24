using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Repositories
{
    public class OAuthUserRepository : IOAuthUserRepository
    {
        private readonly SQLiteConnection _connection;

        public OAuthUserRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void InsertOAuthUser(SQLiteTransaction? transaction, int userId, string email, string token, string provider, string date)
        {
            _connection.Execute($"INSERT INTO OAuthUser (UserId, Email, AccessToken, RefreshToken, Provider, TokenExpiration, CreatedDate) VALUES('{userId}','{email}','{token}','{token}','{provider}','{date}', '{date}')", transaction: transaction);
        }

        public int GetOAuthUserCount()
        {
            return _connection.QueryFirst<int>("SELECT COUNT(*) FROM OAuthUser;");
        }
    }
}
