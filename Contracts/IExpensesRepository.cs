using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IExpensesRepository
    {
        IEnumerable<Expenses> GetAllUserExpenses(Guid id);
        IEnumerable<Expenses> GetUserExpensesWithDetails(Guid id);
        Expenses GetExpenseById(Guid id);
        Expenses GetUserExpenseByDate(Guid userId, long date);
        Expenses GetUserExpenseByDateWithDetails(Guid userId, long date);

        bool ExpenseExists(Guid userId, long date);

        void UpdateExpense(Expenses expense);
        void CreateExpense(Expenses expense);
        void DeleteExpense(Expenses expense);
    }
}
