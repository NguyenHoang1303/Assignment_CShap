using System;
using System.Collections;
using System.Collections.Generic;
using Assignment.Util;

namespace Assignment.Entity
{
    public class Account
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Type { get; set; }
        public bool LockTransaction { get; set; }
        public int Status { get; set; }
        public double Balance { get; set; }

        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeleteAt { get; set; }

        public Account(string fullName, string identityNumber, string phone, string email, string address)
        {
            FullName = fullName;
            IdentityNumber = identityNumber;
            Phone = phone;
            Email = email;
            Address = address;
            UpdateAt = DateTime.Now;
        }

        public Account(string id, string userName, string passwordHash, string salt, string type, bool lockTransaction,
            int status, double balance, string fullName, string identityNumber, string phone, string email,
            string address, DateTime createAt, DateTime updateAt, DateTime deleteAt)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Salt = salt;
            Type = type;
            LockTransaction = lockTransaction;
            Status = status;
            Balance = balance;
            FullName = fullName;
            IdentityNumber = identityNumber;
            Phone = phone;
            Email = email;
            Address = address;
            CreateAt = createAt;
            UpdateAt = updateAt;
            DeleteAt = deleteAt;
        }

       

        public Account()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
            Status = 0;
            Balance = 0;
        }

        public Account(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public override string ToString()
        {
            return
                $"Id: {Id}  ||  UserName: {UserName}  ||  Fullname: {FullName}  ||  Identity number: {IdentityNumber}\n" +
                $"Email: {Email}  ||  Phone: {Phone}  ||  Address: {Address}  ||  Type: {Type}  ||  Balance: {Balance}\n" +
                $"Lock transaction: {LockTransaction}  ||  Status: {Status}  ||  CreateAt: {CreateAt}  ||  UpdateAt: {UpdateAt}";
        }

        public void GetInformationCreatAccount()
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
            Console.WriteLine("Please enter type:");
            Console.WriteLine("1: Personal");
            Console.WriteLine("2: Corporate");
            Type = Console.ReadLine();
            Console.WriteLine("Please enter email:");
            Email = Console.ReadLine();
            Console.WriteLine("Please enter IdentityNumber:");
            IdentityNumber = Console.ReadLine();
            Console.WriteLine("Please enter address:");
            Address = Console.ReadLine();
        }

        public Dictionary<string, string> ValidateInformationUpdate()
        {
            var error = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(FullName))
            {
                error.Add("fullName", "Full name can not be left blank");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                error.Add("phone", "Phone number can not be left blank");
            }

            if (string.IsNullOrEmpty(IdentityNumber))
            {
                error.Add("identityNumber", "IdentityNumber can not be left blank");
            }

            if (string.IsNullOrEmpty(Email))
            {
                error.Add("email", "Email can not be left blank");
            }

            if (string.IsNullOrEmpty(Address))
            {
                error.Add("address", "Address can not be left blank");
            }

            return error;
        }

        public Dictionary<string, string> Validate()
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
                error.Add("confirmPassword", "Confirm password can not be left blank");
            }

            if (!Password.Equals(ConfirmPassword))
            {
                error.Add("checkConfirmPassword", "Confirm password not like password");
            }

            if (string.IsNullOrEmpty(FullName))
            {
                error.Add("fullName", "Full name can not be left blank");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                error.Add("phone", "Phone number can not be left blank");
            }

            if (string.IsNullOrEmpty(Type))
            {
                error.Add("type", "Type can not be left blank");
            }

            if (!Type.Equals("1") && !Type.Equals("2"))
            {
                error.Add("checkType", "Please re-enter Type");
            }

            if (string.IsNullOrEmpty(IdentityNumber))
            {
                error.Add("identityNumber", "IdentityNumber can not be left blank");
            }

            if (string.IsNullOrEmpty(Email))
            {
                error.Add("email", "Email can not be left blank");
            }

            if (string.IsNullOrEmpty(Address))
            {
                error.Add("address", "Address can not be left blank");
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

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                error.Add("confirmPassword", "Confirm password can not be left blank");
            }

            if (!Password.Equals(ConfirmPassword))
            {
                error.Add("checkConfirmPassword","Password and confirm password must be the same");
            }

            return error;
        }

        public void CreatId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void PasswordEncryption()
        {
            Salt = Hash.RandomString(10);
            PasswordHash = Hash.GenerateSaltedSHA1(Password, Salt);
        }
    }
}