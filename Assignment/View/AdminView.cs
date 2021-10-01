using System;
using Assignment.Controller;
using Assignment.Entity;

namespace Assignment.View
{
    public class AdminView : IAdminView
    {
        private readonly AdminController _controller;

        public AdminView()
        {
            _controller = new AdminController();
        }

        public bool Login(string userName, string password)
        {
            return _controller.Login(new Admin(userName, password));
        }

        public void GenerateAdmin()
        {
            var isAdmin = true;
            while (isAdmin)
            {
                Console.WriteLine("*********************************************************************");
                Console.WriteLine($"       Welcome back Admin \"{_controller.Admin.UserName}\". Please enter choice:");
                Console.WriteLine("*********************************************************************");
                Console.WriteLine("1.Creat Admin");
                Console.WriteLine("2.Change Password");
                Console.WriteLine("3.Update Information");
                Console.WriteLine("4.Delete Admin");
                Console.WriteLine("5.Show Admin");
                Console.WriteLine("6.Search Admin");
                Console.WriteLine("7.Show User");
                Console.WriteLine("8.Lock/UnLock/Delete User");
                Console.WriteLine("9.Search User");
                Console.WriteLine("10.Search TransactionHistory by account number");
                Console.WriteLine("11.List Transaction");
                Console.WriteLine("0.Exit");
                Console.WriteLine("*********************************************************************");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        _controller.Create();
                        break;
                    case 2:
                        _controller.ChangeThePassword();
                        break;
                    case 3:
                        _controller.UpdateInformation();
                        break;
                    case 4:
                        _controller.DeleteAdmin();
                        break;
                    case 5:
                        _controller.ShowAdmins();
                        break;
                    case 6:
                        _controller.SearchAdmin();
                        break;
                    case 7:
                        _controller.ShowUsers();
                        break;
                    case 8:
                        _controller.HandlerUser();
                        break;
                    case 9:
                        _controller.SearchUser();
                        break;
                    case 10:
                        _controller.SearchTransactionHistoryByAccountId();
                        break;
                    case 11:
                        _controller.ShowTransactions();
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
    }
}