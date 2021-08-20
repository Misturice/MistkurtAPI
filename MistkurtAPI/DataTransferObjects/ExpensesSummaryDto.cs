using Entities.DataTransferObjects;
using MistkurtAPI.Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class ExpensesSummaryDto
    {
        public IEnumerable<ExpensesDto> Expenses { get; set; }

        public ExpensesDto ExpenseDetails { get; set; }

        public float Total { get; set; }

        public ProductTotals HighestType { get; set; }
        public ProductTotals HighestTag { get; set; }
        public ProductTotals LowestType { get; set; }
        public ProductTotals LowestTag { get; set; }

        //public IEnumerable<ProductDto> Products { get; set; }
    }
}
