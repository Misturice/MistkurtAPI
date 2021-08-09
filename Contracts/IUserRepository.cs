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

        bool EmailExists(string email);

        void Update(User user);
    }
}
