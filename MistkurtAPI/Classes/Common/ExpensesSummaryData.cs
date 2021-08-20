using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Common
{
    public class ExpensesSummaryData
    {
        public object value;

        public ExpensesSummaryData(ExpensesDto val) { value = val; }
        public ExpensesSummaryData(IEnumerable<ExpensesDto> val) { value = val; }

    }
}
