using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {

        }

        public IEnumerable<User> GetAllUsers()
        {
            return FindAll().OrderBy(user => user.Email).ToList();
        }

        public User GetUserById(Guid id)
        {
            return FindByKey(id);
        }

        public User GetUserByEmail(string email)
        {
            return FindByCondition(user => user.Email.Equals(email)).First();
        }

        public bool EmailExists(string email)
        {
            return Exists(user => user.Email.Equals(email));
        }

        public void Update(User user)
        {
          Update(user);
        }

        
    }
}
