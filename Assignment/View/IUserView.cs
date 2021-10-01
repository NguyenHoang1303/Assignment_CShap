namespace Assignment.View
{
    public interface IUserView
    {
        void GenerateUser();
        void Register();
        bool Login(string userName, string password);
    }
}