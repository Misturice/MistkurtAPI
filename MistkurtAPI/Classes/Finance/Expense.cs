using MistkurtAPI.Classes.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Finance
{
    public class Expense
    {
        public IEnumerable<Models.Product> productsList;
        public int expenseID;

        private readonly MistKurtContext _context;
        

        public Expense(MistKurtContext context, int id)
        {
            _context = context;
            expenseID = id;
            FindProductsAsync();
        }


        private async Task FindProductsAsync()
        {
            productsList = await Postgres.FindProductsByExpenseAsync(expenseID, _context);
        }

    }
}
