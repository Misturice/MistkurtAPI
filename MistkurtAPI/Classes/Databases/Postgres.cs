using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistkurtAPI.Models;
namespace MistkurtAPI.Classes.Databases
{
    public static class Postgres
    {

        #region User Models
        public static bool UserExistsByEmail(string email, MistKurtContext context)
        {
            return context.Users.Any(e => e.Email == email);
        }

        public static async Task<User> FindUserByEmailAsync(string email, MistKurtContext context)
        {
            return await context.Users.FindAsync(email);
        }

        public static async Task<User> FindUserByIDAsync(Guid id, MistKurtContext context)
        {
            return await context.Users.FindAsync(id);
        }

        public static async Task<ActionResult<IEnumerable<User>>> ReturnAllUsers(MistKurtContext context)
        {
            return await context.Users.ToListAsync();
        }

        public static async Task AddNewUserAsync(User user, MistKurtContext context)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public static async Task UpdateUser(User user,MistKurtContext context)
        {
            context.Update(user);
            await context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteUser(Guid id, MistKurtContext context)
        {
            User user = await FindUserByIDAsync(id, context);
            if (user == null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region ExpensesModel
        
        public static async Task<ActionResult<Expenses>> GetUserExpenses(Guid id, MistKurtContext context)
        {
            return await context.Expenses.FindAsync(id);
        }

        public static async Task<ActionResult<IEnumerable<Expenses>>> GetUserExpensesByDateAsync(Guid id, int startDate, int endDate, MistKurtContext context)
        {
            return await context.Expenses.Where(elem => elem.UserID == id && elem.Date >= startDate && elem.Date <= endDate).ToListAsync();
        }

        #endregion

        #region ProductsModel
        public static async Task<IEnumerable<Product>> FindProductsByExpenseAsync(int id, MistKurtContext context)
        {
            return await context.Products.Where(elem => elem.ExpensesID == id).ToListAsync();
        }
        #endregion

    }
}
