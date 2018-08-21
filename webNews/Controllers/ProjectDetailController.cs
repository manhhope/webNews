using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    public class ProjectDetailController : BaseController
    {
        // GET: ProjectDetail
        public ActionResult Index()
        {
            return View();
        }
    }
}