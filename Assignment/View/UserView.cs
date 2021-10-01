using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.Controller;
using Assignment.Entity;

namespace Assignment.View
{
    public class UserView: IUserView
    {
        private readonly AccountController _controllerAccount;
        public UserView()
        {
            _controllerAccount = new AccountController();
        }

        public void GenerateUser()
        {
        
        var isAdmin = true;
         while (isAdmin)
         {
             Console.WriteLine("*********************************************************************");
             Console.WriteLine($"          Welcome back \"{_controllerAccount.Account.UserName}\". Please enter choice:");
             Console.WriteLine("*********************************************************************");
             Console.WriteLine("1.Deposit");
             Console.WriteLine("2.Withdraw");
             Console.WriteLine("3.Transfer");
             Console.WriteLine("4.Check Balance");
             Console.WriteLine("5.Update information");
             Console.WriteLine("6.Change password");
             Console.WriteLine("7.Show transactions");
             Console.WriteLine("8.Lock/Unlock transaction");
             Console.WriteLine("0.Exit");
             Console.WriteLine("*********************************************************************");
             var choice = Convert.ToInt32(Console.ReadLine());
             switch (choice)
             {
                 case 1:
                     _controllerAccount.Deposit();
                     break;
                 case 2:
                     _controllerAccount.WithDraw();
                     break;
                 case 3:
                     _controllerAccount.Transfer();
                     break;
                 case 4:
                     _controllerAccount.CheckBalance();
                     break;
                 case 5:
                     _controllerAccount.UpdateInformation();
                     break;
                 case 6:
                     _controllerAccount.ChangePassword();
                     break;
                 case 7:
                     _controllerAccount.CheckTransactionHistory();
                     break;
                 case 8:
                     _controllerAccount.HandlerLockTransaction();
                     break;
                 case 0:
                     isAdmin = false;
                     break;
                 default:
                     Console.WriteLine("Please re-enter choice!");
                     break;
             }
         }
        }

        public void Register()
        {
            _controllerAccount.Register();
        }
        public bool Login(string userName, string password)
        {   
            return _controllerAccount.Login(new Account(userName, password));
        }
    }
}