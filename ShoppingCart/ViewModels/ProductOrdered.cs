using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class ProductOrdered
    {
        public int      ProductID   { set; get; }
        public string   ProductName { set; get; }
        public decimal  Price       { set; get; }
        public int      QtyOrdered  { set; get; }

        public ProductOrdered()
        {
        }

        public ProductOrdered(int productID, string productName, decimal price, int qtyOrdered)
        {
            ProductID   = productID;
            ProductName = productName;
            Price       = price;
            QtyOrdered  = qtyOrdered;
        }
    }
}
