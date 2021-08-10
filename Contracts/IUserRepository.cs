using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        User GetUserWithDetails(Guid id);

        bool EmailExists(string email);

        void UpdateUser(User user);
        void CreateUser(User user);
        void DeleteUser(User user);
    }
}
