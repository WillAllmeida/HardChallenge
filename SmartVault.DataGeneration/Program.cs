using SmartVault.BusinessLogic.Interfaces;
using SmartVault.BusinessLogic.Repositories;
using SmartVault.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration
{
    partial class Program
    {
        static Random _random = new Random();
        static DateTime _start = new DateTime(1985, 1, 1);
        static string _fileName = "TestDoc.txt";

        static SQLiteConnection _connection = null!;
        static IDocumentRepository _documentRepository = null!;
        static IAccountRepository _accountRepository = null!;
        static IUserRepository _userRepository = null!;
        static IOAuthUserRepository _oauthUserRepository = null!;
        static IDatabaseService _databaseService = new DatabaseService();
        static IFileService _fileService = new FileService();

        static void Main(string[] args)
        {
            if (_fileService.VerifyIfFileExists(_databaseService.GetDatabaseName()))
            {
                Console.WriteLine("The database already exists.");
                return;
            }

            _databaseService.CreateDatabaseFile();
            _fileService.CreateFile(_fileName, $"This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}");

            using (_connection = _databaseService.GetDatabaseConnection())
            {
                _connection.Open();
                InitializeRepositories();

                var files = Directory.GetFiles(Path.Join("..", "..", "..", "..", "BusinessObjectSchema"));

                _databaseService.CreateDatabaseTables(_connection, files);

                using (var transaction = _connection.BeginTransaction())
                {
                    Parallel.ForEach(Enumerable.Range(0, 100), new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                    {
                        var randomDayIterator = RandomDay().GetEnumerator();
                        randomDayIterator.MoveNext();
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        _userRepository.InsertUser(transaction, i, randomDayIterator.Current.ToString("yyyy-MM-dd"), currentDate);
                        _accountRepository.InsertAccount(transaction, i, currentDate);

                        for (int d = 0; d < 10000; d++)
                        {
                            _documentRepository.InsertDocument(transaction, i, d, _fileService.GetFileInfo(_fileName), currentDate);

                        }
                    });
                    transaction.Commit();
                }

                Console.WriteLine($"AccountCount: {_accountRepository.GetAccountCount()}");
                Console.WriteLine($"DocumentCount: {_documentRepository.GetDocumentCount()}");
                Console.WriteLine($"UserCount: {_userRepository.GetUserCount()}");
                Console.WriteLine($"OAuthUserCount: {_oauthUserRepository.GetOAuthUserCount()}");
            }
        }

        private static void InitializeRepositories()
        {
            _accountRepository = new AccountRepository(_connection);
            _documentRepository = new DocumentRepository(_connection);
            _userRepository = new UserRepository(_connection);
            _oauthUserRepository = new OAuthUserRepository(_connection);
        }
        static IEnumerable<DateTime> RandomDay()
        {
            int range = (DateTime.Today - _start).Days;
            while (true)
                yield return _start.AddDays(_random.Next(range));
        }
    }

}