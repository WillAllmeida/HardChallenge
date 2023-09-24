using Dapper;
using SmartVault.BusinessLogic.BusinessObjects;
using SmartVault.BusinessLogic.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO.Abstractions;
using System.Linq;

namespace SmartVault.BusinessLogic.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public IEnumerable<string> GetDocumentPathListByAccountId(SQLiteConnection connection, string accountId)
        {
            var result = connection.Query<Document>($"SELECT * FROM Document WHERE AccountID = {accountId};").Select(d => d.FilePath);

            return result;
        }

        public IEnumerable<string> GetDocumentPathList(SQLiteConnection connection)
        {
            var result = connection.Query<string>($"SELECT FilePath FROM Document;");

            return result;
        }

        public void InsertDocument(SQLiteConnection connection, SQLiteTransaction? transaction, int accountId, int documentCount, IFileInfo documentInfo, string currentDate)
        {
            connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document{accountId}-{documentCount}.txt','{documentInfo.FullName}','{documentInfo.Length}','{accountId}', '{currentDate}')", transaction: transaction);
        }

        public int GetDocumentCount(SQLiteConnection connection)
        {
            return connection.QueryFirst<int>("SELECT COUNT(*) FROM Document;");
        }
    }
}
