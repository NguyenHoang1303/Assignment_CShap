using System;
using System.Collections.Generic;
using Assignment.Entity;
using Assignment.Model;
using Assignment.Util;

namespace Assignment.Controller
{
    public class AdminController : IAdminController
    {
        private const string Lock = "LOCK";
        private const string Unlock = "UNLOCK";
        private const string Delete = "DELETE";
        private const string Update = "Update information";
        private const string ChangePassword = "CHANGE PASSWORD";
        private const string Success = "SUCCESS";
        private const string Fail = "FAIL";
        private const string NotFound = "NOT FOUND";
        private const int LimitPagination = 5;
        private int _currentPage = 1;
        private readonly IAdminModel _adminModel;
        private readonly IAccountModel _accountModel;
        private readonly ITransactionHistoryModel _transactionHistoryModel;
        public Admin Admin { get; set; }

        public AdminController()
        {
            _adminModel = new AdminModel();
            _accountModel = new AccountModel();
            _transactionHistoryModel = new TransactionHistoryModel();
        }

        public bool Login(Admin admin)
        {
            while (true)
            {
                var existsAdmin = _adminModel.FindByUserName(admin.UserName);
                var checkPass = existsAdmin != null &&
                                Hash.StringComparison(admin.Password, existsAdmin.PasswordHash, existsAdmin.Salt);
                var errors = new Dictionary<string, string>();
                if (existsAdmin != null)
                {
                    switch (existsAdmin.Status)
                    {
                        case -1:
                            errors.Add("Status", "Deleted account can't login.");
                            break;
                        case 2:
                            errors.Add("Status", "Locked account can't login.");
                            break;
                    }
                }

                if (errors.Count > 0 || !checkPass)
                {
                    foreach (var value in errors.Values)
                    {
                        Console.WriteLine(value);
                    }

                    return false;
                }

                Console.WriteLine("Login Success.");
                Admin = existsAdmin;
                return true;
            }
        }

        public void Create()
        {
            var admin = new Admin();
            bool checkValid;
            do
            {
                admin.GetInformationAdmin();
                var err = admin.ValidateCreatAdmin();
                if (CheckExistUserNameAdmin(admin.UserName))
                {
                    err.Add("username_duplicate", "User name duplicate");
                }

                checkValid = err.Count == 0;
                if (!checkValid)
                {
                    foreach (var value in err.Values)
                    {
                        Console.WriteLine(value);
                    }
                }
            } while (!checkValid);

            admin.CreatIdAdmin();
            while (CheckExistIdAdmin(admin.Id))
            {
                admin.CreatIdAdmin();
            }

            admin.PasswordEncryption();
            Console.WriteLine(_adminModel.Save(admin) ? Success : Fail);
        }

        private bool CheckExistIdAdmin(string id)
        {
            return _adminModel.FindById(id) != null;
        }

        private bool CheckExistUserNameAdmin(string userName)
        {
            return _adminModel.FindByUserName(userName) != null;
        }

        public void ChangeThePassword()
        {
            var admin = Admin;
            do
            {
                Console.WriteLine("Please enter old password:");
                var passwordOld = Console.ReadLine();
                Console.WriteLine("Please enter new password:");
                admin.Password = Console.ReadLine();
                Console.WriteLine("Please enter confirm password:");
                admin.ConfirmPassword = Console.ReadLine();
                Console.WriteLine(admin.ToString());

                var errors = admin.ChangePasswordValidate();
                var checkOldPassword = Hash.StringComparison(passwordOld, admin.PasswordHash, admin.Salt);
                if (!checkOldPassword)
                {
                    errors.Add("PasswordOld_Duplicate", "Old password is not correct");
                }

                if (string.IsNullOrEmpty(passwordOld))
                {
                    errors.Add("PasswordOld", "Old password cannot be blank");
                }

                if (errors.Count > 0)
                {
                    Console.WriteLine(Fail);
                    foreach (var error in errors.Values)
                    {
                        Console.WriteLine(error);
                    }

                    continue;
                }

                var isConfirm = Confirm.HandlerConfirm(ChangePassword);
                if (!isConfirm)
                {
                    break;
                }

                admin.PasswordEncryption();
                Console.WriteLine(_adminModel.ChangePassword(admin.Id, admin.PasswordHash, admin.Salt)
                    ? Success
                    : Fail);
                break;
            } while (true);
        }

        public void UpdateInformation()
        {
            while (true)
            {
                Console.WriteLine("Please enter full name:");
                var fullName = Console.ReadLine();
                Console.WriteLine("Please enter phone:");
                var phone = Console.ReadLine();
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone))
                {
                    Console.WriteLine("Cannot be left blank Full name or Phone");
                    continue;
                }

                Admin.FullName = fullName;
                Admin.Phone = phone;
                break;
            }

            if (!Confirm.HandlerConfirm(Update))
            {
                return;
            }

            Admin.UpdateAt = DateTime.Now;
            Console.WriteLine(Admin.ToString());
            Console.WriteLine(_adminModel.Update(Admin.Id, Admin) ? Success : Fail);
        }

        public void DeleteAdmin()
        {
            var admin = SearchAdmin();
            if (admin == null)
            {
                Console.WriteLine(NotFound);
                return;
            }

            if (!Confirm.HandlerConfirm(Delete))
            {
                return;
            }

            Console.WriteLine(_adminModel.Delete(admin.Id) ? Success : Fail);
        }

        public Admin FindByUsernameAdmin()
        {
            Console.WriteLine("Please enter user name admin:");
            var username = Console.ReadLine();
            var admin = _adminModel.FindByUserName(username);
            Console.WriteLine(admin != null ? admin.ToString() : NotFound);
            return admin;
        }

        public Admin FindByIdAdmin()
        {
            Console.WriteLine("Please enter id admin:");
            var id = Console.ReadLine();
            var admin = _adminModel.FindById(id);
            Console.WriteLine(admin != null ? admin.ToString() : NotFound);
            return admin;
        }

        public Admin SearchAdmin()
        {
            while (true)
            {
                Console.WriteLine("You want to search users by: ");
                Console.WriteLine("1.User name");
                Console.WriteLine("2.Id");
                Console.WriteLine("0.Exit");
                Console.WriteLine("Please enter choice:");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        return FindByUsernameAdmin();
                    case 2:
                        return FindByIdAdmin();
                    case 0:
                        return null;
                    default:
                        Console.WriteLine("Please re-enter choice:");
                        break;
                }
            }
        }

        public void ShowAdmins()
        {
            var sumAdmin = _adminModel.SumAdmin();
            do
            {
                var start = (_currentPage - 1) * LimitPagination;
                var accounts = _adminModel.FindAll(start, LimitPagination);
                if (accounts.Count == 0)
                {
                    Console.WriteLine(NotFound);
                    return;
                }

                if (accounts.Count > 0)
                {
                    foreach (var account in accounts)
                    {
                        Console.WriteLine(
                            "*******************************************************************************************************************");
                        Console.WriteLine(account.ToString());
                    }
                }

                Console.WriteLine($"\n*********** Current page: {_currentPage} ***************\n");
                if (HandleCurrentPage(sumAdmin))
                {
                    break;
                }
            } while (true);
        }

        public void ShowUsers()
        {
            var sumUser = _accountModel.SumUser();
            do
            {
                var start = (_currentPage - 1) * LimitPagination;
                var accounts = _accountModel.FindAll(start, LimitPagination);
                if (accounts.Count == 0)
                {
                    Console.WriteLine(NotFound);
                    return;
                }

                if (accounts.Count > 0)
                {
                    foreach (var account in accounts)
                    {
                        Console.WriteLine(
                            "*******************************************************************************************************************");
                        Console.WriteLine(account.ToString());
                    }
                }

                Console.WriteLine($"\n*********** Current page: {_currentPage} ***************\n");
                if (HandleCurrentPage(sumUser))
                {
                    break;
                }
            } while (true);
        }

        public void LockUser(Account account)
        {
            account.Status = 2;
            Console.WriteLine(_accountModel.HandlerLockAccount(account.Id, account.Status) ? Success : Fail);
        }

        public void UnLockUser(Account account)
        {
            account.Status = 1;
            Console.WriteLine(_accountModel.HandlerLockAccount(account.Id, account.Status) ? Success : Fail);
        }

        public void DeleteUser(Account account)
        {
            account.Status = -1;
            Console.WriteLine(_accountModel.DeleteAccount(account.Id, account.Status) ? Success : Fail);
        }

        public void HandlerUser()
        {
            var account = SearchUser();
            if (account == null)
            {
                return;
            }

            var isHandlerUser = true;
            while (isHandlerUser)
            {
                Console.WriteLine("What do you want:");
                Console.WriteLine("1.Lock ");
                Console.WriteLine("2.Unlock ");
                Console.WriteLine("3.Delete ");
                Console.WriteLine("0.Exit ");
                Console.WriteLine("Please enter choice:");
                var choice = Convert.ToInt32(Console.ReadLine());
                bool isConfirm;
                switch (choice)
                {
                    case 1:
                        isConfirm = Confirm.HandlerConfirm(Lock);
                        if (isConfirm)
                        {
                            LockUser(account);
                        }

                        isHandlerUser = false;
                        break;
                    case 2:
                        isConfirm = Confirm.HandlerConfirm(Unlock);
                        if (isConfirm)
                        {
                            UnLockUser(account);
                        }

                        isHandlerUser = false;
                        break;
                    case 3:
                        isConfirm = Confirm.HandlerConfirm(Delete);
                        if (isConfirm)
                        {
                            DeleteUser(account);
                        }

                        isHandlerUser = false;
                        break;
                    case 0:
                        isHandlerUser = false;
                        break;
                    default:
                        Console.WriteLine("Please re-enter choice:");
                        break;
                }
            }
        }

        public Account SearchUser()
        {
            while (true)
            {
                Console.WriteLine("You want to search users by: ");
                Console.WriteLine("1.Id");
                Console.WriteLine("2.User name");
                Console.WriteLine("3.Identity number");
                Console.WriteLine("4.Phone");
                Console.WriteLine("0.Exit");
                Console.WriteLine("Please enter choice:");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        return SearchUserByAccountId();
                    case 2:
                        return SearchByAccountUsername();
                    case 3:
                        return SearchUserByIdentityNumber();
                    case 4:
                        return FindUserByPhone();
                    case 0:
                        return null;
                    default:
                        Console.WriteLine("Please re-enter choice:");
                        break;
                }
            }
        }

        public Account FindUserByPhone()
        {
            Console.WriteLine("Please enter phone");
            var phone = Console.ReadLine();
            var account = _accountModel.FindByPhone(phone);
            Console.WriteLine(account != null ? account.ToString() : NotFound);
            return account;
        }

        public Account SearchUserByAccountId()
        {
            Console.WriteLine("Please enter AccountId");
            var accountId = Console.ReadLine();
            var account = _accountModel.FindByAccountId(accountId);
            Console.WriteLine(account != null ? account.ToString() : NotFound);
            return account;
        }

        public Account SearchByAccountUsername()
        {
            Console.WriteLine("Please enter Username Account");
            var accountUsername = Console.ReadLine();
            var account = _accountModel.FindByUserName(accountUsername);
            Console.WriteLine(account != null ? account.ToString() : NotFound);
            return account;
        }

        public Account SearchUserByIdentityNumber()
        {
            Console.WriteLine("Please enter Identity number");
            var identityNumber = Console.ReadLine();
            var account = _accountModel.FindByIdentityNumber(identityNumber);
            Console.WriteLine(account != null ? account.ToString() : NotFound);
            return account;
        }

        public void SearchTransactionHistoryByAccountId()
        {
            Console.WriteLine("Please enter AccountId:");
            var accountId = Console.ReadLine();
            if (string.IsNullOrEmpty(accountId))
            {
                Console.WriteLine("AccountId cannot be left blank");
                return;
            }
            do
            {
                var sumTransaction = _transactionHistoryModel.SumTransactionByAccountId(accountId);
                if (sumTransaction == 0)
                {
                    Console.WriteLine(NotFound);
                    Console.WriteLine("Please check your accountId again");
                    break;
                }
                var start = (_currentPage - 1) * LimitPagination;
                var transactions = _transactionHistoryModel.FindByAccountId(accountId, start, LimitPagination);
                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        "*******************************************************************************************************************");
                    Console.WriteLine(transaction.ToString());
                }

                Console.WriteLine($"\n*********** Current page: {_currentPage} ***************\n");
                if (HandleCurrentPage(sumTransaction))
                {
                    break;
                }
            } while (true);
        }

        public void ShowTransactions()
        {
            var sumTransaction = _transactionHistoryModel.SumTransaction();
            do
            {
                var start = (_currentPage - 1) * LimitPagination;
                var transactions = _transactionHistoryModel.FindAll(start, LimitPagination);
                if (transactions.Count == 0)
                {
                    Console.WriteLine(NotFound);
                    return;
                }

                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        "*******************************************************************************************************************");
                    Console.WriteLine(transaction.ToString());
                }

                Console.WriteLine($"\n*********** Current page: {_currentPage} ***************\n");
                if (HandleCurrentPage(sumTransaction))
                {
                    break;
                }
            } while (true);
        }

        private bool HandleCurrentPage(int totalPage)
        {
            var totalRecord = totalPage / LimitPagination + 1;
            var exit = false;
            Console.WriteLine("enter '->' next page:");
            Console.WriteLine("enter '<-' previous page:");
            Console.WriteLine("enter 'Tab' exit:");
            var readKey = Console.ReadKey();
            switch (readKey.Key)
            {
                case ConsoleKey.RightArrow:
                    _currentPage++;
                    if (_currentPage > totalRecord)
                    {
                        _currentPage = 1;
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    _currentPage--;
                    if (_currentPage < 1)
                    {
                        _currentPage = totalRecord;
                    }

                    break;
                case ConsoleKey.Tab:
                    _currentPage = 1;
                    exit = true;
                    break;
            }

            return exit;
        }
    }
}