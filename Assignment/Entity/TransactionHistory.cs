using System;
using System.Collections.Generic;

namespace Assignment.Entity
{
    public class TransactionHistory
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string SenderAccountId { get; set; }
        public string ReceiverAccountId { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public int Status { get; set; }

        public TransactionHistory(string accountId, string senderAccountId)
        {
            AccountId = accountId;
            SenderAccountId = senderAccountId;
            CreateAt = DateTime.Now;
        }
        
        public TransactionHistory(string id, string accountId, string senderAccountId, string receiverAccountId,
            int type, double amount, string message, DateTime createAt, int status)
        {
            Id = id;
            AccountId = accountId;
            SenderAccountId = senderAccountId;
            ReceiverAccountId = receiverAccountId;
            Type = type;
            Amount = amount;
            Message = message;
            CreateAt = createAt;
            Status = status;
        }
        
        public TransactionHistory(string accountId, string senderAccountId, string receiverAccountId, int type,
            double amount, string message, int status)
        {
            AccountId = accountId;
            SenderAccountId = senderAccountId;
            ReceiverAccountId = receiverAccountId;
            Type = type;
            Amount = amount;
            Message = message;
            Status = status;
            CreateAt = DateTime.Now;
        }

        public TransactionHistory()
        {
        }
        
        public override string ToString()
        {
            return $"Id: {Id}                 |        AccountId: {AccountId},\n" +
                   $"SenderAccountId: {SenderAccountId}    |    ReceiverAccountId: {ReceiverAccountId}, \n" +
                   $"Type: {HandlerType()}    |   Amount: {Amount}                        |    Message: {Message},\n" +
                   $"CreateAt: {CreateAt}                           |    Status: {HandlerStatus()}";
        }

        private string HandlerStatus()
        {
            switch (Status)
            {
                case 1:
                    return "Success";
                case 2:
                    return "Processing";
                case 0:
                    return "Fail";
                default:
                    return null;
            }
        }
        private string HandlerType()
        {
            switch (Type)
            {
                case 1:
                    return "Deposit";
                case 2:
                    return "Withdraw";
                case 3:
                    return "Tranfer";
                default:
                    return null;
            }
        }

        public void GetInformationTranfer()
        {
            Console.WriteLine("Please enter the receiving account number:");
            ReceiverAccountId = Console.ReadLine();
            Console.WriteLine("Please enter message:");
            Message = Console.ReadLine();
            Console.WriteLine("Please enter the amount you want to tranfer:");
            Amount = Convert.ToDouble(Console.ReadLine());
        }
        public Dictionary<string, string> ValidateTransfer()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(ReceiverAccountId))
            {
                errors.Add("ReceiverAccountId", "ReceiverAccountId can not be left blank");
            }

            if (string.IsNullOrEmpty(Message))
            {
                errors.Add("Message", "Message can not be left blank");
            }

            return errors;
        }
        public void CreatId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}