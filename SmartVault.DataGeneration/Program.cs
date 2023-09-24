using SmartVault.BusinessLogic;
using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration
{
    partial class Program
    {
        static Random _random = new Random();
        static DateTime _start = new DateTime(1985, 1, 1);
        static DatabaseHelper _databaseHelper = new DatabaseHelper();
        static FileHelper _fileHelper = new FileHelper();
        static string _fileName = "TestDoc.txt";
        static IDocumentRepository _documentRepository = new DocumentRepository();
        static IAccountRepository _accountRepository = new AccountRepository();
        static IUserRepository _userRepository = new UserRepository();

        static void Main(string[] args)
        {
            if (_fileHelper.VerifyIfFileExists(_databaseHelper.GetDatabaseName()))
            {
                Console.WriteLine("The database already exists.");
                return;
            }

            _databaseHelper.CreateDatabaseFile();
            _fileHelper.CreateFile(_fileName, $"This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}");

            using (var connection = _databaseHelper.GetDatabaseConnection())
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                connection.Open();
                var files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");

                _databaseHelper.CreateDatabaseTables(connection, files);

                using (var transaction = connection.BeginTransaction())
                {
                    Parallel.ForEach(Enumerable.Range(0, 100), i =>
                    {
                        var randomDayIterator = RandomDay().GetEnumerator();
                        randomDayIterator.MoveNext();
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        _userRepository.InsertUser(connection, transaction, i, randomDayIterator.Current.ToString("yyyy-MM-dd"), currentDate);
                        _accountRepository.InsertAccount(connection, transaction, i, currentDate);

                        for (int d = 0; d < 10000; d++)
                        {
                            _documentRepository.InsertDocument(connection, transaction, i, d, _fileHelper.GetFileInfo(_fileName), currentDate);

                        }
                    });
                    transaction.Commit();
                }
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

                Console.WriteLine($"AccountCount: {_accountRepository.GetAccountCount(connection)}");
                Console.WriteLine($"DocumentCount: {_documentRepository.GetDocumentCount(connection)}");
                Console.WriteLine($"UserCount: {_userRepository.GetUserCount(connection)}");
            }
        }

        static IEnumerable<DateTime> RandomDay()
        {
            int range = (DateTime.Today - _start).Days;
            while (true)
                yield return _start.AddDays(_random.Next(range));
        }
    }

}