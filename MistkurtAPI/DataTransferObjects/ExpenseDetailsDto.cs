using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class ExpenseDetailsDto
    {
        public ExpensesDto Expense { get; set; }

        public ProductTotals HighestType { get; set; }
        public ProductTotals HighestTag { get; set; }
        public ProductTotals LowestType { get; set; }
        public ProductTotals LowestTag { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
    }
}
