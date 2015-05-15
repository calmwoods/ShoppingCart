using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class VisitRepo
    {
        public bool InsertVisit(Visit visit)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            db.Visits.Add(visit);
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

        public bool DeleteVisitBySessionID(string sessionID)
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            var visits = db.Visits.Where(r => r.sessionID == sessionID);
            if (visits != null)
            {
                try
                {
                    foreach (var each in visits)
                    {
                        db.Visits.Remove(each);
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

        public bool DeleteExpiredVisit()
        {
            ShoppingCartEntities db = new ShoppingCartEntities();
            DateTime expireTime = DateTime.Now.AddHours(-1);
            var visits = db.Visits.Where(r => r.started <= expireTime);
            if (visits != null)
            {
                try
                {
                    foreach (var each in visits)
                    {
                        db.Visits.Remove(each);
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
    }
}