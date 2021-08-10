using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {

        }

        public IEnumerable<User> GetAllUsers()
        {
            return FindAll().
                OrderBy(user => user.Name)
                .ToList();
        }

        public User GetUserById(Guid id)
        {
            return FindByKey(id);
        }

        public User GetUserWithDetails(Guid id)
        {
            return FindByCondition(user => user.Id.Equals(id))
                .Include(e => e.Expenses)
                .ThenInclude(e => e.Products)
                .FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return FindByCondition(user => user.Email.Equals(email))
                .FirstOrDefault();
        }

        public bool EmailExists(string email)
        {
            return Exists(user => user.Email.Equals(email));
        }

        public void UpdateUser(User user)
        {
          Update(user);
        }

        public void CreateUser(User user)
        {
            Create(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        
    }
}
