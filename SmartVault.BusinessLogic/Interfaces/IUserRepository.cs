using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IUserRepository
    {
        void InsertUser(SQLiteTransaction? transaction, int id, string randomDate, string currentDate);
        int GetUserCount();
    }
}
