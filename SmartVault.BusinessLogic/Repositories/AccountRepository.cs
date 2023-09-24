using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SQLiteConnection _connection;

        public AccountRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void InsertAccount(SQLiteTransaction? transaction, int i, string currentDate)
        {
            _connection.Execute($"INSERT INTO Account (Id, Name, CreatedDate) VALUES('{i}','Account{i}', '{currentDate}')", transaction: transaction);
        }

        public int GetAccountCount()
        {
            return _connection.QueryFirst<int>("SELECT COUNT(*) FROM Account;");
        }
    }
}
