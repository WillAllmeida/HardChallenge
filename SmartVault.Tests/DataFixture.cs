using System;
using System.IO;

namespace SmartVault.Tests
{
    public abstract class DataFixture
    {
        internal static string[] _files = new string[4] { "Account.xml", "User.xml", "Document.xml", "OAuthUser.xml" };
        internal static string _filesDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Files/";
        internal readonly string _directory = Path.GetFullPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\..\\");
    }
}
