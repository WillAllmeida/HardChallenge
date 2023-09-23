using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Reflection.Metadata;

namespace SmartVault.BusinessLogic
{
    public class DatabaseHelper
    {
        private readonly IConfigurationRoot _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

        private readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");
        public SQLiteConnection GetDatabaseConnection()
        {
            return new SQLiteConnection(string.Format(_configuration?["ConnectionStrings:DefaultConnection"] ?? "", _directory + _configuration["DatabaseFileName"]));
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_configuration["DatabaseFileName"]);
        }

        public string GetDocumentListByAccountId(SQLiteConnection connection, string accountId)
        {
            var result = connection.Query($"SELECT * FROM Document WHERE AccountID = {accountId};");

            return JsonConvert.SerializeObject(result);
        }
    }
}
