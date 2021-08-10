using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ExpensesDto
    {
        public long Date { get; set; }
        public float Total { get; set; }
        public Guid Id { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
