using System.Data.SQLite;

namespace SmartVault.BusinessLogic.Interfaces
{
    public interface IOAuthUserRepository
    {
        void InsertOAuthUser(SQLiteTransaction? transaction, int userId, string email, string token, string provider, string date);
        int GetOAuthUserCount();
    }
}
