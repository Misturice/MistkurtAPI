using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class ProductTotals : IProductTotals
    {
        public string Key { get; set; }
        public float Total { get; set; }
    }
}
