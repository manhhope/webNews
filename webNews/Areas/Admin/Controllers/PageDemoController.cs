using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Areas.Admin.Models;
using webNews.Language.Language;

namespace webNews.Areas.Admin.Controllers
{
    public class PageDemoController : Controller
    {
        // GET: Admin/PageDemo
        public ActionResult Index()
        {
            ViewBag.breadcrumb = new List<Breadcrumb>{
                new Breadcrumb {Title = Resource.Homepage, Url = "", Active = false},
                new Breadcrumb {Title = "Page Demo", Url = "#", Active = true},
            };
            return View();
        }
    }
}