using System;
using System.IO;

namespace SmartVault.Tests
{
    public abstract class DataFixture
    {
        internal static string[] _files = new string[3] { "Account.xml", "User.xml", "Document.xml" };
        internal static string _filesDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Files/";
        internal readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");
    }
}
