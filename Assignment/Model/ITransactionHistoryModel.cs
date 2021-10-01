using System.Collections.Generic;
using System.Transactions;
using Assignment.Entity;

namespace Assignment.Model
{
    public interface ITransactionHistoryModel
    {
        bool Save(TransactionHistory transactionHistory);
        List<TransactionHistory> FindAll(int page, int limit);
        int SumTransaction();
        int SumTransactionByAccountId(string accountID);
        TransactionHistory FindById(string transactionId);
        List<TransactionHistory> FindByAccountId(string accountId, int start, int limit);
        
    }
}