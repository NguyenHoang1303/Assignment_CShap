using System;
using System.Collections.Generic;
using Assignment.Entity;
using Assignment.Util;
using MySql.Data.MySqlClient;

namespace Assignment.Model
{
    public class AdminModel : IAdminModel
    {
        private const string InsertQuery =
            "insert into admins (id, username,password_hash, salt, fullname, phone, createAt, updateAt, status )"
            + "values (@id, @username, @password_hash, @salt, @fullname, @phone, @createAt, @updateAt, @status )";

        private const string QueryId = "select * from admins where id = @id";

        private const string QueryUsername = "select * from admins where username = @username";

        private const string DeleteQuery = "update admins set status = -1 where id = @id";


        private const string UpdateQuery =
            "Update admins Set fullname = @fullname, phone = @phone, updateAt = @updateAt, status = @status  Where id = @id";

        private const string ChangePasswordQuery =
            "Update admins Set password_hash = @password_hash, salt = @salt Where id = @id";

        private const string SumQuery = "select COUNT(*) from admins";

        public bool Save(Admin account)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(InsertQuery, cnn);
                    command.Parameters.AddWithValue("@username", account.UserName);
                    command.Parameters.AddWithValue("@id", account.Id);
                    command.Parameters.AddWithValue("@password_hash", account.PasswordHash);
                    command.Parameters.AddWithValue("@salt", account.Salt);
                    command.Parameters.AddWithValue("@fullname", account.FullName);
                    command.Parameters.AddWithValue("@phone", account.Phone);
                    command.Parameters.AddWithValue("@createAt", account.CreatAt);
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

        public bool Update(string id, Admin updateAccount)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(UpdateQuery, cnn);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@fullname", updateAccount.FullName);
                    command.Parameters.AddWithValue("@phone", updateAccount.Phone);
                    command.Parameters.AddWithValue("@updateAt", updateAccount.UpdateAt);
                    command.Parameters.AddWithValue("@status", Convert.ToInt32(updateAccount.Status));
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

        public bool ChangePassword(string id, string passwordHash, string salt)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(ChangePasswordQuery, cnn);
                    command.Parameters.AddWithValue("@id", id);
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

        public bool Delete(string id)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(DeleteQuery, cnn);
                    command.Parameters.AddWithValue("@id", id);
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

        public Admin FindById(string id)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryId, cnn);
                    command.Parameters.AddWithValue("@id", id);
                    command.Prepare();
                    var data = command.ExecuteReader();
                    return data.Read() ? GetAdminByQuery(data) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Admin FindByUserName(string username)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryUsername, cnn);
                    command.Parameters.AddWithValue("@username", username);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    return reader.Read() ? GetAdminByQuery(reader) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int SumAdmin()
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

        public List<Admin> FindAll(int start, int limit)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var queryLimit = $"select * from admins limit {start}, {limit}";
                    var command = new MySqlCommand(queryLimit, cnn);
                    command.Prepare();
                    var data = command.ExecuteReader();
                    var admins = new List<Admin>();
                    while (data.Read())
                    {
                        admins.Add(GetAdminByQuery(data));
                    }

                    return admins;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private Admin GetAdminByQuery(MySqlDataReader data)
        {
            var id = data.GetString("id");
            var username = data.GetString("username");
            var fullname = data.GetString("fullname");
            var phone = data.GetString("phone");
            var createdAt = data.GetDateTime("createAt");
            var updateAt = data.GetDateTime("updateAt");
            var status = data.GetInt32("status");
            var passwordHash = data.GetString("password_hash");
            var salt = data.GetString("salt");
            return new Admin(id, username, passwordHash, salt,
                fullname, phone, createdAt, updateAt, status);
        }
    }
}