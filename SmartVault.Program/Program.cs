using Dapper;
using Newtonsoft.Json;
using SmartVault.BusinessLogic;
using SmartVault.Program.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartVault.Program
{
    partial class Program
    {
        static DatabaseHelper _databaseHelper = new DatabaseHelper();
        static FileHelper _fileHelper = new FileHelper();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                GetAllFileSizes();
                WriteEveryThirdFileToFile("");
                return;
            }

        }

        private static void GetAllFileSizes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            long totalSize = 0;
            var pathCache = new Dictionary<string, long>();

            using (var connection = _databaseHelper.GetDatabaseConnection())
            {
                var queryResult = _databaseHelper.GetDocumentPathList(connection);
                foreach (var path in queryResult)
                {
                    if (pathCache.TryGetValue(path, out long currentSize))
                    {
                        totalSize += currentSize;
                    }
                    else
                    {
                        long size = _fileHelper.GetFileSize(path);
                        pathCache[path] = size;
                        totalSize += size;
                    }
                }
            }

            Console.WriteLine("Total Size: " + totalSize);
        }

        private static void WriteEveryThirdFileToFile(string accountId)
        {
            var sb = new StringBuilder();
            using (var connection = _databaseHelper.GetDatabaseConnection())
            {
                var queryResult = _databaseHelper.GetDocumentListByAccountId(connection, "3");
                var documents = JsonConvert.DeserializeObject<IEnumerable<Document>>(queryResult);
                for (int i = 0; i < documents?.Count() / 3; i = i + 3)
                {
                    sb.Append(_fileHelper.CheckStringInFile(documents?.ElementAt(i).FilePath, "Smith Property"));
                }

                _fileHelper.CreateFile("result.txt", sb.ToString());
                Console.WriteLine("The new file has been created");
            }
        }
    }
}