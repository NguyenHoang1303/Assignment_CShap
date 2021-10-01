using System;
using System.Collections.Generic;
using System.IO;
using Assignment.Entity;

namespace Assignment.Model
{
    public interface IAccountModel
    {
        bool Save(Account account); //đăng kí tài khoản
        bool Update(string accountId, Account updateAccount); // sửa thông tin tài khoản
        bool ChangePassword(string accountId, string passwordHash, string salt);
        bool HandlerLockTransaction(string accountId, bool lockTransaction);
        bool HandlerLockAccount(string accountId, int status);
        bool DeleteAccount(string accountId, int status);
        Account FindByAccountId(string accountId); //tìm kiếm tk 
        Account FindByUserName(string accountUserName);
        List<Account> FindAll(int start, int limit); // lấy danh sách tk kèm phân trang
        Account FindByPhone(string keyword); // search theo sđt

        Account FindByIdentityNumber(string keyword); // search theo cccd/cmnd
        //sao kê

        bool HandlerBalanceTransaction(string accountNumber, double balanceUpdate); // gửi tiền
        int SumUser();
    }
}