using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class ProductVisitRepo
    {
        public ProductVisit GetOneProductVisit(string sessionID, int productId)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            ProductVisit productVisit = (db.ProductVisits
                                .Where(r => r.sessionID == sessionID && r.productID == productId))
                                .FirstOrDefault();
            return productVisit;
        }

        public bool InsertProductVisit(ProductVisit productVisit)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            db.ProductVisits.Add(productVisit);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateProductVisit(ProductVisit productVisit)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            ProductVisit pv = (db.ProductVisits
                                .Where(r => r.sessionID == productVisit.sessionID &&
                                            r.productID == productVisit.productID))
                                .FirstOrDefault();
            try
            {
                pv.qtyOrdered = (int)productVisit.qtyOrdered;
                pv.updated = DateTime.Now;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool DeleteProductVisitBySessionID(string sessionID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            var productVists = db.ProductVisits.Where(r => r.sessionID == sessionID);

            if (productVists != null)
            {
                try
                {
                    foreach (var each in productVists)
                    {
                        db.ProductVisits.Remove(each);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DeleteProductVisit(string sessionID, int productID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            var productVists = db.ProductVisits.Where(r => r.sessionID == sessionID &&
                                                           r.productID == productID);
            if (productVists != null)
            {
                try
                {
                    foreach (var each in productVists)
                    {
                        db.ProductVisits.Remove(each);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DeleteExpiredProductVisit()
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            DateTime expireTime = DateTime.Now.AddHours(-1);
            var productVisits = db.ProductVisits.Where(r => r.updated <= expireTime);
            if (productVisits != null)
            {
                try
                {
                    foreach (var each in productVisits)
                    {
                        db.ProductVisits.Remove(each);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetOrderCount(string sessionID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            return db.ProductVisits.Where(r => r.sessionID == sessionID).Count();
        }
    }
}