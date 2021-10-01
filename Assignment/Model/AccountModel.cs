using System;
using System.Collections.Generic;
using Assignment.Entity;
using Assignment.Util;
using MySql.Data.MySqlClient;

namespace Assignment.Model
{
    public class AccountModel : IAccountModel
    {
        private const string InsertQuery =
            "insert into accounts "
            + "(id, username, password_hash, salt, type, lock_transaction, balance, fullname, identity_number, phone, email, address, createAt, updateAt, status )"
            + "values (@id ,@username, @password_hash, @salt, @type, @lock_transaction, @balance, @fullname, @identity_number, @phone, @email, @address, @createAt, @updateAt, @status )";

        private const string QueryId = "select * from accounts where id = @id";
        private const string QueryUsername = "select * from accounts where username = @username";
        private const string QueryPhone = "select * from accounts where phone = @phone";
        private const string QueryIdentity = "select * from accounts where identity_number = @identity_number";

        private const string DepositTransaction = "Update accounts set balance = @balance where id = @id";

        private const string ChangePasswordQuery =
            "Update accounts Set password_hash = @password_hash, salt = @salt Where id = @id";

        private const string LockTransactionQuery =
            "Update accounts Set lock_transaction = @lock_transaction Where id = @id";

        private const string LockAccountQuery =
            "Update accounts Set status = @status Where id = @id";
        private const string DeleteQuery =
            "Update accounts Set status = @status, deleteAt = @deleteAt  Where id = @id";

        private const string UpdateQuery =
            "Update accounts set fullname = @fullname, identity_number = @identity_number," +
             "phone = @phone, email = @email, address = @address, updateAt = @updateAt where id = @id";

        private const string SumQuery = "select COUNT(*) from accounts";
        
        public bool Save(Account account)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(InsertQuery, cnn);
                    command.Parameters.AddWithValue("@id", account.Id);
                    command.Parameters.AddWithValue("@username", account.UserName);
                    command.Parameters.AddWithValue("@password_hash", account.PasswordHash);
                    command.Parameters.AddWithValue("@salt", account.Salt);
                    command.Parameters.AddWithValue("@type", Convert.ToInt32(account.Type));
                    command.Parameters.AddWithValue("@lock_transaction", account.LockTransaction);
                    command.Parameters.AddWithValue("@balance", account.Balance);
                    command.Parameters.AddWithValue("@fullname", account.FullName);
                    command.Parameters.AddWithValue("@identity_number", account.IdentityNumber);
                    command.Parameters.AddWithValue("@phone", account.Phone);
                    command.Parameters.AddWithValue("@email", account.Email);
                    command.Parameters.AddWithValue("@address", account.Address);
                    command.Parameters.AddWithValue("@createAt", account.CreateAt);
                    command.Parameters.AddWithValue("@updateAt", account.UpdateAt);
                    command.Parameters.AddWithValue("@status", account.Status);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Update(string accountId, Account updateAccount)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(UpdateQuery, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Parameters.AddWithValue("@fullname", updateAccount.FullName);
                    command.Parameters.AddWithValue("@identity_number", updateAccount.IdentityNumber);
                    command.Parameters.AddWithValue("@phone", updateAccount.Phone);
                    command.Parameters.AddWithValue("@email", updateAccount.Email);
                    command.Parameters.AddWithValue("@address", updateAccount.Address);
                    command.Parameters.AddWithValue("@updateAt", updateAccount.UpdateAt);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ChangePassword(string accountId, string passwordHash, string salt)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(ChangePasswordQuery, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Parameters.AddWithValue("@password_hash", passwordHash);
                    command.Parameters.AddWithValue("@salt", salt);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool HandlerLockTransaction(string accountId, bool lockTransaction)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(LockTransactionQuery, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Parameters.AddWithValue("@lockTransaction", lockTransaction);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool HandlerLockAccount(string accountId, int status)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(LockAccountQuery, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Parameters.AddWithValue("@status", status);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool DeleteAccount(string accountId, int status)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(DeleteQuery, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@deleteAd", DateTime.Now);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public Account FindByAccountId(string accountId)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryId, cnn);
                    command.Parameters.AddWithValue("@id", accountId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    return reader.Read() ? GetAccountByQuery(reader) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Account FindByUserName(string accountUserName)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryUsername, cnn);
                    command.Parameters.AddWithValue("@username", accountUserName);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    return reader.Read() ? GetAccountByQuery(reader) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Account FindByPhone(string keyword)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryPhone, cnn);
                    command.Parameters.AddWithValue("@phone", keyword);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    return reader.Read() ? GetAccountByQuery(reader) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<Account> FindAll(int start, int limit)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var queryLimit = $"select * from accounts limit {start}, {limit}";
                    var command = new MySqlCommand(queryLimit, cnn);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    var accounts = new List<Account>();
                    while (reader.Read())
                    {
                        accounts.Add(GetAccountByQuery(reader));
                    }

                    return accounts;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Account FindByIdentityNumber(string keyword)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryIdentity, cnn);
                    command.Parameters.AddWithValue("@identity_number", keyword);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    return reader.Read() ? GetAccountByQuery(reader) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool HandlerBalanceTransaction(string accountNumber, double balanceUpdate)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(DepositTransaction, cnn);
                    command.Parameters.AddWithValue("@id", accountNumber);
                    command.Parameters.AddWithValue("@balance", balanceUpdate);
                    command.Prepare();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int SumUser()
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(SumQuery, cnn);
                    var data = command.ExecuteScalar();
                    var count = 0;
                    if (data != null)
                    {
                        count = Convert.ToInt32(data);
                    }

                    return count;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private Account GetAccountByQuery(MySqlDataReader dataReader)
        {
            var id = dataReader.GetString("id");
            var username = dataReader.GetString("username");
            var passwordHash = dataReader.GetString("password_hash");
            var salt = dataReader.GetString("salt");
            var type = Convert.ToString(dataReader.GetInt32("type"));
            var lockTransaction = dataReader.GetBoolean("lock_transaction");
            var status = dataReader.GetInt32("status");
            var balance = dataReader.GetInt32("balance");

            var fullname = dataReader.GetString("fullname");
            var identityNumber = dataReader.GetString("identity_number");
            var phone = dataReader.GetString("phone");
            var email = dataReader.GetString("email");
            var address = dataReader.GetString("address");
            var createdAt = dataReader.GetDateTime("createAt");
            var updateAt = dataReader.GetDateTime("updateAt");
            var deleteAt = dataReader.GetDateTime("deleteAt");

            return new Account(id, username, passwordHash, salt,
                type, lockTransaction, status, balance, fullname,
                identityNumber, phone, email, address, createdAt, updateAt, deleteAt);
        }
    }
}