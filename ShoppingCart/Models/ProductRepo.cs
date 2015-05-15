using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ShoppingCart.ViewModels;

namespace ShoppingCart.Models
{
    public class ProductRepo
    {
        public IEnumerable<Product> GetAllProducts()
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            IEnumerable<Product> products = db.Products;
            return products; 
        }

        public Product GetOneProduct(int productID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            Product product = (db.Products
                                .Where(r => r.productID == productID))
                                .FirstOrDefault();
            return product;
        }

        public IEnumerable<ProductOrdered> GetProductsOrdered(string sessionID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            var orders = (from p in db.Products
                          from pv in p.ProductVisits
                          where p.productID == pv.productID && pv.sessionID == sessionID
                          select new ProductOrdered
                          {
                              ProductID = p.productID,
                              ProductName = p.productName,
                              Price = (decimal)p.price,
                              QtyOrdered = (int)pv.qtyOrdered
                          }).ToList();
            return orders;
        }
    }
}