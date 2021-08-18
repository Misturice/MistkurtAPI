using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class ExpensesDashboardDto
    {
        public IEnumerable<ExpensesDto> Expenses { get; set; } 

        public float Total { get; set; }

        public ProductTotals HighestType { get; set; }
        public ProductTotals HighestTag { get; set; }
        public ProductTotals LowestType { get; set; }
        public ProductTotals LowestTag { get; set; }
    }
}