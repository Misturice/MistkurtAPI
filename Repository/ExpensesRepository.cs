using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ExpensesRepository : RepositoryBase<Expenses>, IExpensesRepository
    {
        public ExpensesRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateExpense(Expenses expense)
        {
            Create(expense);
        }

        public void DeleteExpense(Expenses expense)
        {
            DeleteExpense(expense);
        }

        public bool ExpenseExists(Guid userId, long date)
        {
            return Exists(elem => elem.UserId.Equals(userId) && elem.Date.Equals(date));
        }

        public IEnumerable<Expenses> GetAllUserExpenses(Guid userId)
        {
            return FindByCondition(elem => elem.UserId.Equals(userId)).ToList();
        }

        public Expenses GetExpenseById(Guid id)
        {
            return FindByKey(id);
        }

        public IEnumerable<Expenses> GetUserExpensesWithDetails(Guid id)
        {
            return FindByCondition(elem => elem.UserId.Equals(id))
                .Include(elem => elem.Products)
                .OrderBy(elem => elem.Date)
                .ToList();
        }

        public void UpdateExpense(Expenses expense)
        {
            Update(expense);
        }

        public Expenses GetUserExpenseByDate(Guid userId, long date)
        {
            return FindByCondition(elem => elem.UserId.Equals(userId) && elem.Date.Equals(date)).FirstOrDefault();
        }

        public Expenses GetUserExpenseByDateWithDetails(Guid userId, long date)
        {
            return FindByCondition(elem => elem.UserId.Equals(userId) && elem.Date.Equals(date))
                .Include(elem => elem.Products)
                .FirstOrDefault();
        }

        public IEnumerable<Expenses> GetUserExpensesByRange(Guid userId, long startDate, long endDate)
        {
            return FindByCondition(elem => elem.UserId.Equals(userId) && elem.Date >= startDate && elem.Date <= endDate)
                .OrderBy(elem => elem.Date)
                .ToList();
        }

        public IEnumerable<Expenses> GetUserExpensesByRangeWithDetails(Guid userId, long startDate, long endDate)
        {
            return FindByCondition(elem => elem.UserId.Equals(userId) && elem.Date >= startDate && elem.Date <= endDate)
                .Include(elem => elem.Products)
                .OrderBy(elem => elem.Date)
                .ToList();
        }
    }
}
