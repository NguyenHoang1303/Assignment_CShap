using System.Collections.Generic;
using Assignment.Entity;

namespace Assignment.Model
{
    public interface IAdminModel
    {
        bool Save(Admin account);
        bool Update(string id ,Admin updateAccount);
        bool ChangePassword(string id ,string passwordHash, string salt);
        bool Delete(string id);
        Admin FindById(string id);
        List<Admin> FindAll(int start, int limit);

        Admin FindByUserName(string username);
        int SumAdmin();
    }
}