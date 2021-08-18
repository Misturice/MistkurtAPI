using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductTotals
    {
        public string Key { get; set; }
        public float Total { get; set; }
    }
}
