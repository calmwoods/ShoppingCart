using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using ShoppingCart.Models;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string sessionID = HttpContext.Current.Session.SessionID;
            ProductVisitRepo productVisitRepo   = new ProductVisitRepo();
            VisitRepo visitRepo                 = new VisitRepo();

            productVisitRepo.DeleteExpiredProductVisit();
            visitRepo.DeleteExpiredVisit();

            productVisitRepo.DeleteProductVisitBySessionID(sessionID);
            visitRepo.DeleteVisitBySessionID(sessionID);

            Visit visit     = new Visit();
            visit.sessionID = sessionID;
            visit.started   = DateTime.Now;
            visitRepo.InsertVisit(visit);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ProductVisitRepo productVisitRepo   = new ProductVisitRepo();
            VisitRepo visitRepo                 = new VisitRepo();
            productVisitRepo.DeleteExpiredProductVisit();
            visitRepo.DeleteExpiredVisit();
        }
    }
}
