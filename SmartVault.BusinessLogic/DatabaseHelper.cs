using Dapper;
using Microsoft.Extensions.Configuration;
using SmartVault.Library;
using System;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

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

        public void CreateDatabaseFile()
        {
            SQLiteConnection.CreateFile(_directory + _configuration["DatabaseFileName"]);
        }

        public void CreateDatabaseTables(SQLiteConnection connection, string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var serializer = new XmlSerializer(typeof(BusinessObject));
                var businessObject = serializer.Deserialize(new StreamReader(files[i])) as BusinessObject;
                connection.Execute(businessObject?.Script);
            }
        }

        public string GetDatabaseName()
        {
            return _configuration["DatabaseFileName"];
        }
    }
}
