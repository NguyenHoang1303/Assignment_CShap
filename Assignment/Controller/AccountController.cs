using System;
using System.Collections.Generic;
using Assignment.Entity;
using Assignment.Model;
using Assignment.Util;

namespace Assignment.Controller
{
    public class AccountController : IAccountController
    {
        private const string Success = "SUCCESS";
        private const string Fail = "FAIL";
        private const string NotFound = "Not found please try again.";
        private readonly IAccountModel _accountModel;
        private readonly ITransactionHistoryModel _transactionHistoryModel;
        private int _currentPage = 1;
        private const int LimitPagination = 5;
        public Account Account { get; set; }


        public AccountController()
        {
            _accountModel = new AccountModel();
            _transactionHistoryModel = new TransactionHistoryModel();
        }

        public void Register()
        {
            var account = new Account();
            while (true)
            {
                account.GetInformationCreatAccount();
                var err = account.Validate();
                if (CheckExistUserName(account.UserName))
                {
                    err.Add("username_duplicate", "User name duplicate");
                }

                var checkValid = err.Count == 0;
                if (!checkValid)
                {
                    foreach (var value in err.Values)
                    {
                        Console.WriteLine(value);
                    }

                    continue;
                }

                break;
            }

            account.CreatId();
            while (CheckExistAccountNumber(account.Id))
            {
                account.CreatId();
            }

            account.PasswordEncryption();
            Console.WriteLine(account.ToString());
            Console.WriteLine(_accountModel.Save(account) ? "Creat Success" : "Fail");
        }

        public bool Login(Account account)
        {
            while (true)
            {
                var errors = new Dictionary<string, string>();
                var existAccount = _accountModel.FindByUserName(account.UserName);
                var isLogin = existAccount != null &&
                              Hash.StringComparison(account.Password, existAccount.PasswordHash, existAccount.Salt);
                if (existAccount != null)
                {
                    switch (existAccount.Status)
                    {
                        case -1:
                            errors.Add("Status", "Deleted account can't login.");
                            break;
                        case 2:
                            errors.Add("Status", "Locked account can't login.");
                            break;
                    }
                }

                if (errors.Count > 0 || !isLogin)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }

                    return false;
                }

                Account = existAccount;
                Console.WriteLine("Login Success!");
                return true;
            }
        }

        private bool CheckExistAccountNumber(string accountId)
        {
            return _accountModel.FindByAccountId(accountId) != null;
        }

        private bool CheckExistUserName(string accountUserName)
        {
            return _accountModel.FindByUserName(accountUserName) != null;
        }

        public void Deposit()
        {
            Console.WriteLine("Please enter the amount to deposit:");
            var amount = Convert.ToDouble(Console.ReadLine());
            Account.Balance += amount;
            var isDeposit = _accountModel.HandlerBalanceTransaction(Account.Id, Account.Balance);
            var message = isDeposit ? Success : Fail;
            var status = isDeposit ? 1 : 0;
            var accountId = Account.Id;
            var transaction = new TransactionHistory
                (accountId, accountId, accountId, 1, amount, message, status);

            do
            {
                transaction.CreatId();
            } while (CheckExistIdTransaction(transaction.Id));

            _transactionHistoryModel.Save(transaction);
            Console.WriteLine(isDeposit ? "Deposit " + Success : "Deposit " + Fail);
        }

        public void WithDraw()
        {
            double amount;
            while (true)
            {
                Console.WriteLine("Please enter the amount you want to withdraw:");
                amount = Convert.ToDouble(Console.ReadLine());
                if (Account.Balance < amount)
                {
                    Console.WriteLine("Insufficient balance");
                    Console.WriteLine("Please re-enter");
                    continue;
                }

                break;
            }
            if (!Confirm.HandlerConfirm("Withdraw")) return;
            Account.Balance -= amount;
            var isWithdraw = _accountModel.HandlerBalanceTransaction(Account.Id, Account.Balance);
            var message = isWithdraw ? Success : Fail;
            var status = isWithdraw ? 1 : 0;
            var accountId = Account.Id;
            var transaction = new TransactionHistory
                (accountId, accountId, accountId, 2, amount, message, status);

            do
            {
                transaction.CreatId();
            } while (CheckExistIdTransaction(transaction.Id));

            _transactionHistoryModel.Save(transaction);
            Console.WriteLine(isWithdraw ? "WithDraw " + Success : "WithDraw " + Fail);
        }

        public void Transfer()
        {
            var transaction = new TransactionHistory
                (Account.Id, Account.Id);
            double amount;
            Account receiveAccount;

            while (true)
            {
                transaction.GetInformationTranfer();
                var errors = transaction.ValidateTransfer();

                amount = transaction.Amount;
                if (Account.Balance < amount)
                {
                    errors.Add("balance", "Insufficient balance");
                }

                receiveAccount = _accountModel.FindByAccountId(transaction.ReceiverAccountId);
                if (receiveAccount == null)
                {
                    errors.Add("ReceiveAccountId", "Receive AccountId does not exist");
                }

                if (receiveAccount != null && receiveAccount.LockTransaction)
                {
                    errors.Add("LockTransaction", "The receiver has locked the transaction");
                    return;
                }

                var checkTranfer = errors.Count == 0;
                if (!checkTranfer)
                {
                    foreach (var value in errors.Values)
                    {
                        Console.WriteLine(value);
                    }

                    continue;
                }

                break;
            }

            if (!Confirm.HandlerConfirm("Transfer")) return;
            var checkHandlerBalance = HandlerBalanceTranfer(Account, receiveAccount, amount);
            transaction.Status = checkHandlerBalance ? 1 : 0;
            transaction.Type = 3;

            do
            {
                transaction.CreatId();
            } while (CheckExistIdTransaction(transaction.Id));

            Console.WriteLine(_transactionHistoryModel.Save(transaction)
                ? "Creat transaction success"
                : "Creat transaction fail");
            Console.WriteLine(checkHandlerBalance ? "Transfer " + Success : "Transfer " + Fail);
        }

        private bool CheckExistIdTransaction(string transactionId)
        {
            return _transactionHistoryModel.FindById(transactionId) != null;
        }

        private bool HandlerBalanceTranfer(Account accountSender, Account receiveAccount, double amount)
        {
            // update số dư người gửi sau khi chuyển tiền
            return _accountModel.HandlerBalanceTransaction(accountSender.Id,
                       accountSender.Balance -= amount) &&
                   // update số dư người nhận  sau khi nhận đưuọc tiền
                   _accountModel.HandlerBalanceTransaction(receiveAccount.Id,
                       receiveAccount.Balance += amount);
        }

        public void CheckInformation()
        {
            Console.WriteLine(Account.ToString());
        }

        public void UpdateInformation()
        {
            while (true)
            {
                var account = GetInformationUpdate();
                var errors = account.ValidateInformationUpdate();
                if (errors.Count > 0)
                {
                    foreach (var value in errors.Values)
                    {
                        Console.WriteLine(value);
                    }

                    continue;
                }

                Console.WriteLine(_accountModel.Update(Account.Id, account) ? Success : Fail);
                break;
            }
        }

        private Account GetInformationUpdate()
        {
            Console.WriteLine("Please enter full name:");
            var fullName = Console.ReadLine();
            Console.WriteLine("Please enter phone:");
            var phone = Console.ReadLine();
            Console.WriteLine("Please enter email:");
            var email = Console.ReadLine();
            Console.WriteLine("Please enter IdentityNumber:");
            var identityNumber = Console.ReadLine();
            Console.WriteLine("Please enter address:");
            var address = Console.ReadLine();
            return new Account(fullName, identityNumber, phone, email, address);
        }

        public void ChangePassword()
        {
            do
            {
                Console.WriteLine("Please enter old password:");
                var passwordOld = Console.ReadLine();
                Console.WriteLine("Please enter new password:");
                Account.Password = Console.ReadLine();
                Console.WriteLine("Please enter confirm password:");
                Account.ConfirmPassword = Console.ReadLine();
                Console.WriteLine(Account.ToString());

                var errors = Account.ChangePasswordValidate();
                var checkOldPassword = Hash.StringComparison(passwordOld, Account.PasswordHash, Account.Salt);
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

                var isConfirm = Confirm.HandlerConfirm("ChangePassword");
                if (!isConfirm)
                {
                    break;
                }

                Account.PasswordEncryption();
                Console.WriteLine(_accountModel.ChangePassword(Account.Id, Account.PasswordHash, Account.Salt)
                    ? Success
                    : Fail);
                break;
            } while (true);
        }

        public void CheckBalance()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Your balance is: {Account.Balance}");
            Console.WriteLine("---------------------------------------");
        }

        public void LockTransaction()
        {
            Account.LockTransaction = true;
            Console.WriteLine(_accountModel.HandlerLockTransaction(Account.Id, Account.LockTransaction)
                ? Success
                : Fail);
        }

        public void UnLockTransaction()
        {
            Account.LockTransaction = false;
            Console.WriteLine(_accountModel.HandlerLockTransaction(Account.Id, Account.LockTransaction)
                ? Success
                : Fail);
        }

        public void HandlerLockTransaction()
        {
            var isHandlerTransaction = true;
            while (isHandlerTransaction)
            {
                Console.WriteLine("what do you want:");
                Console.WriteLine("1.Lock transaction");
                Console.WriteLine("2.Unlock transaction");
                Console.WriteLine("0.Exit");
                Console.WriteLine("Please enter choice:");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        LockTransaction();
                        break;
                    case 2:
                        UnLockTransaction();
                        break;
                    case 0:
                        isHandlerTransaction = false;
                        break;
                    default:
                        Console.WriteLine("Please re-enter.");
                        break;
                }
            }
        }

        public void CheckTransactionHistory()
        {
            do
            {
                var sumTransaction = _transactionHistoryModel.SumTransactionByAccountId(Account.Id);
                if (sumTransaction == 0)
                {
                    Console.WriteLine(NotFound);
                    break;
                }

                var start = (_currentPage - 1) * LimitPagination;
                var transactions = _transactionHistoryModel.FindByAccountId(Account.Id, start, LimitPagination);
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

        private bool HandleCurrentPage(int sumRecord)
        {
            var totalPage = sumRecord / LimitPagination + 1;
            var exit = false;
            Console.WriteLine("enter '->' next page:");
            Console.WriteLine("enter '<-' previous page:");
            Console.WriteLine("enter 'Tab' exit:");
            var readKey = Console.ReadKey();
            switch (readKey.Key)
            {
                case ConsoleKey.RightArrow:
                    _currentPage++;
                    if (_currentPage > totalPage)
                    {
                        _currentPage = 1;
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    _currentPage--;
                    if (_currentPage < 1)
                    {
                        _currentPage = totalPage;
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