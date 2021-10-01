using Assignment.Entity;

namespace Assignment.Controller
{
    public interface IAdminController
    {
        bool Login(Admin admin);
        void Create();
        void ShowAdmins();
        void UpdateInformation();
        void DeleteAdmin();
        void ChangeThePassword();
        Admin FindByUsernameAdmin();
        Admin FindByIdAdmin();
        void ShowUsers();
        void LockUser(Account account);
        void UnLockUser(Account account);
        void DeleteUser(Account account);
        Account FindUserByPhone();
        Account SearchUserByAccountId();
        Account SearchByAccountUsername();
        Account SearchUserByIdentityNumber();
        void SearchTransactionHistoryByAccountId(); // nhập thông tin tìm kiếm bao gồm cả accountNumber
        void ShowTransactions(); // nhập thông tin tìm kiếm bao gồm cả accountNumber

    }
}