using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLiteConnection _connection;

        public UserRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void InsertUser(SQLiteTransaction? transaction, int id, string randomDate, string currentDate)
        {
            _connection.Execute($"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedDate) VALUES('{id}','FName{id}','LName{id}','{randomDate}','{id}','UserName-{id}','e10adc3949ba59abbe56e057f20f883e', '{currentDate}')", transaction: transaction);
        }

        public int GetUserCount()
        {
            return _connection.QueryFirst<int>("SELECT COUNT(*) FROM User;");
        }
    }
}
