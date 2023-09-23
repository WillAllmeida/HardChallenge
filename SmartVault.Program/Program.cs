using Dapper;
using Newtonsoft.Json;
using SmartVault.BusinessLogic;
using SmartVault.Program.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartVault.Program
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            WriteEveryThirdFileToFile(args[0]);
            GetAllFileSizes();
        }

        private static void GetAllFileSizes()
        {
            // TODO: Implement functionality
        }

        private static void WriteEveryThirdFileToFile(string accountId)
        {
            var database = new DatabaseHelper();
            var fileHelper = new FileHelper();
            var sb = new StringBuilder();
            using (var connection = database.GetDatabaseConnection())
            {
                var queryResult = database.GetDocumentListByAccountId(connection, "3");
                var documents = JsonConvert.DeserializeObject<IEnumerable<Document>>(queryResult);
                for(int i = 0; i < documents?.Count() / 3; i = i + 3)
                {
                    sb.Append(fileHelper.CheckStringInFile(documents?.ElementAt(i).FilePath, "Smith Property"));
                }

                fileHelper.CreateFile("result.txt", sb.ToString());
            }
        }
    }
}