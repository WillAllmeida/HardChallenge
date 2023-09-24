using Dapper;
using SmartVault.BusinessLogic.Interfaces;
using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public void InsertAccount(SQLiteConnection connection, SQLiteTransaction? transaction, int i, string currentDate)
        {
            connection.Execute($"INSERT INTO Account (Id, Name, CreatedDate) VALUES('{i}','Account{i}', '{currentDate}')", transaction: transaction);
        }

        public int GetAccountCount(SQLiteConnection connection)
        {
            return connection.QueryFirst<int>("SELECT COUNT(*) FROM Account;");
        }
    }
}
