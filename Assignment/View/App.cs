using System;

namespace Assignment.View
{
    public class App
    {
        private readonly IAdminView _adminView;
        private readonly IUserView _userView;

        public App()
        {
            _adminView = new AdminView();
            _userView = new UserView();
        }

        public void Start()
        {
            var isApp = true;
            while (isApp)
            {
                Console.WriteLine("------Spring Hero Bank------");
                Console.WriteLine("1.Register");
                Console.WriteLine("2.Login");
                Console.WriteLine("0.Exit");
                Console.WriteLine("-----------------------------");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        _userView.Register();
                        break;
                    case 2:
                        HandlerLogin();
                        break;
                    case 0:
                        isApp = false;
                        break;
                    default:
                        Console.WriteLine("Please enter choice");
                        break;
                }
            }
        }

        private void HandlerLogin()
        {
            string userName;
            string password;
            while (true)
            {
                Console.WriteLine("Please enter User name:");
                userName = Console.ReadLine();
                Console.WriteLine("Please enter password:");
                password = Console.ReadLine();
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("User name or Password can not be left blank");
                    continue;
                }

                break;
            }

            if (_userView.Login(userName, password))
            {
                _userView.GenerateUser();
            }else if (_adminView.Login(userName, password))
            {
                _adminView.GenerateAdmin();
            }
            else
            {
                Console.WriteLine("Login fail");
            }
        }
    }
}