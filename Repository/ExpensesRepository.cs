using Contracts;
using Entities;
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
    }
}
