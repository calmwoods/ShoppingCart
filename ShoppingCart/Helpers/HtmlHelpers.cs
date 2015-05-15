using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ShoppingCart.Models;
using ShoppingCart.BusinessLogic;

namespace ShoppingCart.Helpers
{
    public class HtmlHelpers
    {
        public static int OrderCount()
        {
            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo();
            int orderCount = productVisitRepo.GetOrderCount(sessionID);

            return orderCount; 
        }

    }
}