using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IAccountRepository
    {
        void InsertAccount(SQLiteConnection connection, SQLiteTransaction? transaction, int id, string currentDate);
        int GetAccountCount(SQLiteConnection connection);
    }
}
