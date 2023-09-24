using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IAccountRepository
    {
        void InsertAccount(SQLiteTransaction? transaction, int id, string currentDate);
        int GetAccountCount();
    }
}
