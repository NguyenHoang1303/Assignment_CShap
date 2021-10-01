using System;
using System.Collections.Generic;
using Assignment.Entity;
using Assignment.Util;
using MySql.Data.MySqlClient;

namespace Assignment.Model
{
    public class TransactionHistoryModel : ITransactionHistoryModel
    {
        private const string InsertQuery =
            "insert into transaction_history (id, accountId,senderAccountId, receiverAccountId, type, amount, message, status, createAt )"
            + "values (@id, @accountId, @senderAccountId, @receiverAccountId, @type, @amount, @message, @status, @createAt )";

        private const string QueryId = "select * from transaction_history where id = @id";

        private const string SumQuery = "select COUNT(*) from transaction_history";

        private const string SumQueryByAccountId =
            "select COUNT(*) from transaction_history where accountId = @accountId";

        public bool Save(TransactionHistory transaction)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(InsertQuery, cnn);
                    command.Parameters.AddWithValue("@id", transaction.Id);
                    command.Parameters.AddWithValue("@accountId", transaction.AccountId);
                    command.Parameters.AddWithValue("@senderAccountId", transaction.SenderAccountId);
                    command.Parameters.AddWithValue("@receiverAccountId", transaction.ReceiverAccountId);
                    command.Parameters.AddWithValue("@type", transaction.Type);
                    command.Parameters.AddWithValue("@amount", transaction.Amount);
                    command.Parameters.AddWithValue("@message", transaction.Message);
                    command.Parameters.AddWithValue("@createAt", transaction.CreateAt);
                    command.Parameters.AddWithValue("@status", transaction.Status);
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

        public List<TransactionHistory> FindAll(int start, int limit)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var queryAll = $"select * from transaction_history limit {start}, {limit}";
                    var command = new MySqlCommand(queryAll, cnn);
                    var data = command.ExecuteReader();
                    var transactionsById = new List<TransactionHistory>();
                    while (data.Read())
                    {
                        transactionsById.Add(GetTransactionByQuery(data));
                    }

                    return transactionsById;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int SumTransaction()
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

        public int SumTransactionByAccountId(string accountId)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(SumQueryByAccountId, cnn);
                    command.Parameters.AddWithValue("@accountId", accountId);
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

        public TransactionHistory FindById(string transactionId)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var command = new MySqlCommand(QueryId, cnn);
                    command.Parameters.AddWithValue("@id", transactionId);
                    command.Prepare();
                    var data = command.ExecuteReader();
                    return data.Read() ? GetTransactionByQuery(data) : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<TransactionHistory> FindByAccountId(string accountId, int start, int limit)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnect())
                {
                    cnn.Open();
                    var queryAccountId =
                        $"select * from transaction_history where accountId = @accountId limit {start}, {limit}";
                    var command = new MySqlCommand(queryAccountId, cnn);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Prepare();
                    var data = command.ExecuteReader();
                    var transactionsById = new List<TransactionHistory>();
                    while (data.Read())
                    {
                        transactionsById.Add(GetTransactionByQuery(data));
                    }

                    return transactionsById;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private TransactionHistory GetTransactionByQuery(MySqlDataReader data)
        {
            var id = data.GetString("id");
            var accountId = data.GetString("accountId");
            var senderAccountId = data.GetString("senderAccountId");
            var receiverAccountId = data.GetString("receiverAccountId");
            var type = data.GetInt32("type");
            var amount = data.GetDouble("amount");
            var message = data.GetString("message");
            var status = data.GetInt32("status");
            var createAt = data.GetDateTime("createAt");
            return new TransactionHistory(id, accountId, senderAccountId, receiverAccountId, type, amount, message,
                createAt, status);
        }
    }
}