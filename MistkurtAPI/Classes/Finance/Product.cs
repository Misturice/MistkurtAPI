using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Finance
{
    public class Product
    {
        private Models.Product _productInfo;
        public Models.Product ProductInfo { get => _productInfo; set => _productInfo = value; }

        public Product(Models.Product product)
        {
            ProductInfo = product;
        }

    }
}
