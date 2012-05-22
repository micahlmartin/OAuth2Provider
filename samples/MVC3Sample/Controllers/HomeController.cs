using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Consumer = MvcApplication.ServiceLocator.ConsumerRepository.GetByConsumerId(1);
            ViewBag.ResourceOwner = MvcApplication.ServiceLocator.ResourceOwnerRepository.GetByResourceOwnerId(1, 1);
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
