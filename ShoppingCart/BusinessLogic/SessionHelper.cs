using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BusinessLogic
{
    public class SessionHelper
    {
        public string SessionID
        {
            get
            {
                if (HttpContext.Current.Session.SessionID != null)
                    return HttpContext.Current.Session.SessionID;
                return null;
            }
        }

        public void Clear()
        {
            if (SessionID != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}