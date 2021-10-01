using Assignment.Entity;

namespace Assignment.Controller
{
    public interface IAccountController
    {
        void Register();
        bool Login(Account account);
        void Deposit();
        void WithDraw();
        void Transfer();
        void CheckInformation();
        void UpdateInformation();
        void ChangePassword();
        void CheckBalance();
        void LockTransaction();
        void UnLockTransaction();
        void CheckTransactionHistory();
    }
}