namespace SmartVault.BusinessLogic.BusinessObjects
{
    public partial class User
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}
