using System;
using System.Collections.Generic;
using System.Security.Policy;
using Hash = Assignment.Util.Hash;

namespace Assignment.Entity
{
    public class Admin
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime CreatAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public int Status { get; set; }

        public Admin()
        {
            CreatAt = DateTime.Now;
            UpdateAt = DateTime.Now;
            Status = 1;
        }

        public override string ToString()
        {
            
            return $"id: {Id} | UserName: {UserName} | FullName: {FullName} | phone: {Phone}\n" +
                   $"creatAt: {CreatAt} | UpdateAt: {UpdateAt} | status: {HandlerStatus()}, ";
        }

        public Admin(string id, string userName, string passwordHash, string salt, string fullName, string phone,
            DateTime creatAt, DateTime updateAt, int status)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Salt = salt;
            FullName = fullName;
            Phone = phone;
            CreatAt = creatAt;
            UpdateAt = updateAt;
            Status = status;
        }

        public Admin(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public void GetInformationAdmin()
        {
            Console.WriteLine("Please enter User name:");
            UserName = Console.ReadLine();
            Console.WriteLine("Please enter password:");
            Password = Console.ReadLine();
            Console.WriteLine("Please enter confirm password:");
            ConfirmPassword = Console.ReadLine();
            Console.WriteLine("Please enter full name:");
            FullName = Console.ReadLine();
            Console.WriteLine("Please enter phone:");
            Phone = Console.ReadLine();
        }

     


        public Dictionary<string, string> ValidateCreatAdmin()
        {
            var error = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(UserName))
            {
                error.Add("username", "User name can not be left blank");
            }

            if (string.IsNullOrEmpty(Password))
            {
                error.Add("password", "Password can not be left blank");
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                error.Add("confirmPassword", "Password can not be left blank");
            }

            if (!Password.Equals(ConfirmPassword))
            {
                error.Add("checkConfirmPassword","Password and confirm password must be the same");
            }

            if (string.IsNullOrEmpty(FullName))
            {
                error.Add("fullName", "Full name can not be left blank");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                error.Add("phone", "Phone can not be left blank");
            }

            return error;
        }

        public Dictionary<string, string> ChangePasswordValidate()
        {
            var error = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(Password))
            {
                error.Add("password", "Password can not be left blank");
            }

            if (string.IsNullOrEmpty(ConfirmPassword) || !Password.Equals(ConfirmPassword))
            {
                error.Add("confirmPassword", "Confirm password can not be left blank");
            }

            return error;
        }

        public void PasswordEncryption()
        {
            Salt = Hash.RandomString(9);
            PasswordHash = Hash.GenerateSaltedSHA1(Password, Salt);
        }


        public void CreatIdAdmin()
        {
            Id = Guid.NewGuid().ToString();
        }

        private string HandlerStatus()
        {
            var result = "";
            switch (Status)
            {
                case 1:
                    result = "active";
                    break;
                case 2:
                    result = "lock";
                    break;
                case -1:
                    result = "deleted";
                    break;
            }

            return result;
        }
    }
}