using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Services;

namespace webNews.Controllers
{
    public class NewsDetailController : Controller
    {
        private readonly ISystemService _systemService;

        public NewsDetailController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: NewsDetail
        public ActionResult Index()
        {
            return View();
        }
    }
}