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
        private readonly SQLiteConnection _connection;

        public DocumentRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<string> GetDocumentPathListByAccountId(string accountId)
        {
            var result = _connection.Query<Document>($"SELECT * FROM Document WHERE AccountID = {accountId};").Select(d => d.FilePath);

            return result;
        }

        public IEnumerable<string> GetDocumentPathList()
        {
            return _connection.Query<string>($"SELECT FilePath FROM Document;"); ;
        }

        public void InsertDocument(SQLiteTransaction? transaction, int accountId, int documentCount, IFileInfo documentInfo, string currentDate)
        {
            _connection.Execute($"INSERT INTO Document (Name, FilePath, Length, AccountId, CreatedDate) VALUES('Document{accountId}-{documentCount}.txt','{documentInfo.FullName}','{documentInfo.Length}','{accountId}', '{currentDate}')", transaction: transaction);
        }

        public int GetDocumentCount()
        {
            return _connection.QueryFirst<int>("SELECT COUNT(*) FROM Document;");
        }
    }
}
