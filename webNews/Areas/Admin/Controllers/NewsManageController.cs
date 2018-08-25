using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Services;

namespace webNews.Areas.Admin.Controllers
{
    public class NewsManageController : Controller
    {
        private readonly ISystemService _systemService;

        public NewsManageController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: Admin/NewsManage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddNews()
        {
            return null;
        }
    }
}