using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistkurtAPI.Models;


namespace MistkurtAPI.Classes.Databases
{
    public class Postgres
    {
        MistKurtContext _context;

        public Postgres(MistKurtContext context)
        {
            _context = context;
        }

        #region User Models
        public bool UserExistsByEmail(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FindAsync(email);
        }

        public async Task<User> FindUserByIDAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public  ActionResult<IEnumerable<User>> ReturnAllUsers()
        {
            return  _context.Users.ToList();
        }

        public async Task AddNewUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            User user = await FindUserByIDAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region ExpensesModel
        
        public IEnumerable<Expenses> GetUserExpensesAsync(Guid id)
        {
            return _context.Expenses.Where(elem => elem.UserID == id).ToList();
        }

        public async Task<Expenses> GetExpenseAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<ActionResult<IEnumerable<Expenses>>> GetUserExpensesByDateAsync(Guid id, long startDate, long endDate)
        {
            return await _context.Expenses.Where(elem => elem.UserID == id && elem.Date >= startDate && elem.Date <= endDate).ToListAsync();
        }


        public Expenses GetUserExpenseForGivenDay(Guid id, long date)
        {
            IEnumerable<Expenses> exp = _context.Expenses.Where(elem => elem.UserID == id && elem.Date == date);
            return exp.FirstOrDefault();
        }

        public void AddNewExpense(Expenses expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        #endregion

        #region ProductsModel
        public async Task<IEnumerable<Product>> FindProductsByExpenseAsync(int id)
        {
            return await _context.Products.Where(elem => elem.ExpensesID == id).ToListAsync();
        }

        public async Task<Product> FindProductByIDAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> DeleteProductByID(int id)
        {
            Product product = await FindProductByIDAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }


        #endregion

        #region Common
        public async Task SaveDatabase()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

    }
}
