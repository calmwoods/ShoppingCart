using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ShoppingCart.Models;
using ShoppingCart.BusinessLogic;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {      
        public ActionResult Index()
        { 
            ProductRepo productRepo = new ProductRepo();
            IEnumerable<Product> allProducts = productRepo.GetAllProducts();
            return View(allProducts);
        }

        [HttpGet]
        public ActionResult Add(int id = -1)
        {
            const int DEFAULT_QTY = 1; 
            
            if(id == -1)
            {
                return RedirectToAction("Index", "Error", new { msg="parameter needed: id"});
            }

            ProductRepo productRepo = new ProductRepo();
            Product product = productRepo.GetOneProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Error", new { msg = "invalid id value:" + id });
            }

            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo();
            ProductVisit productVisit = productVisitRepo.GetOneProductVisit(sessionID, id);
            if (productVisit == null)
            {
                ViewBag.qtyOrdered = DEFAULT_QTY;
                ViewBag.productVisitExist = 0; 
            }
            else
            {
                ViewBag.qtyOrdered = productVisit.qtyOrdered;
                ViewBag.productVisitExist = 1; 
            }

            return View(product); 
        }

        [HttpPost]
        public ActionResult Add(int productID, int productVisitExist, int qtyOrdered)
        {
            if (productID <= 0 || qtyOrdered <= 0)
            {
                return RedirectToAction("Index", "Error", 
                            new { msg = "Parameters, productID and qtyOrdered, should be greater than 0" });
            }

            SessionHelper sessionHelper = new SessionHelper();
            string SessionID = sessionHelper.SessionID; 

            ProductVisit productVisit = new ProductVisit();
            productVisit.sessionID = SessionID;
            productVisit.productID = productID;
            productVisit.qtyOrdered = qtyOrdered;
            productVisit.updated = DateTime.Now;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo(); 
            if (productVisitExist == 0)
            {
                if (!productVisitRepo.InsertProductVisit(productVisit))
                {
                    return RedirectToAction("Index", "Error", new { msg="repo.InsertProductVisit failed"});
                }
            }
            else
            {
                if (!productVisitRepo.UpdateProductVisit(productVisit))
                {
                     return RedirectToAction("Index", "Error", new { msg="repo.UpdateProductVisit failed"});
                }
            }

            return RedirectToAction("ViewCart", "Home") ;  
        }

        public ActionResult ViewCart()
        {
            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;
            ProductRepo repo = new ProductRepo();
            IEnumerable<ProductOrdered> productsOrdered = repo.GetProductsOrdered(sessionID);
            ViewBag.orderedCount = productsOrdered.Count();
            return View(productsOrdered); 
        }

        public ActionResult Thankyou()
        {
            return View(); 
        }

        public ActionResult Cancel()
        {
            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo();
            if (!productVisitRepo.DeleteProductVisitBySessionID(sessionID))
            {
                return RedirectToAction("Index", "Error", new { msg = "repo.DeleteProductVisit failed" });
            }

            sessionHelper.Clear();

            return RedirectToAction("Thankyou");
        }

        public ActionResult SaveOrder(List<ProductOrdered> orders)
        {
            if (orders == null)
            {
                return RedirectToAction("Index", "Error", new { msg = "Parameter missing:List<ProductOrdered>" });
            }

            foreach(ProductOrdered order in orders)
            {
                if (order.ProductID <=0 || order.QtyOrdered <=0)
                {
                    return RedirectToAction("Index", "Error", 
                                new { msg = "Parameters, productID and qtyOrdered, should be greater than 0" });
                }
            }

            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo();
            ProductVisit productVisit = new ProductVisit();
            foreach(ProductOrdered order in orders)
            {
                productVisit.sessionID = sessionID;
                productVisit.productID = order.ProductID;
                productVisit.qtyOrdered = order.QtyOrdered;
                if (!productVisitRepo.UpdateProductVisit(productVisit))
                {
                    return RedirectToAction("Index", "Error", new { msg = "repo.UpdateProductVisit failed" });
                }
            }
            return RedirectToAction("ViewCart");
        }

        public ActionResult RemoveOrder(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index", "Error", new { msg = "parameter error:productID" });
            }

            SessionHelper sessionHelper = new SessionHelper();
            string sessionID = sessionHelper.SessionID;

            ProductVisitRepo productVisitRepo = new ProductVisitRepo();
            if (!productVisitRepo.DeleteProductVisit(sessionID, id))
            {
                return RedirectToAction("Index", "Error", new { msg = "repo.DeleteProductVisit failed" });
            }
            return RedirectToAction("ViewCart"); 
        }
    }
}