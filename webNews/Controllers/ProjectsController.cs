using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Services;
using webNews.Security;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly ISystemService _systemService;

        public ProjectsController(ISystemService systemService)
        {
            _systemService = systemService;
        }
        // GET: Projects
        [GZipOrDeflate]
        [OutputCache(CacheProfile = "StaticPage")]
        public ActionResult Index()
        {
            //if (!CheckAuthorizer.IsAuthenticated())
            //    return RedirectToAction("Index", "Login", new { Area = "Admin" });
            var newsCategorieId = Convert.ToInt32(HttpContext.Request.Params.Get("cateId"));
            var page = Convert.ToInt32(HttpContext.Request.Params.Get("page"));

            var filter = new webNews.Models.Filter
            {
                Page = page - 1 < 0 ? 0 : page - 1,
                CateId = newsCategorieId,
                Lang = Authentication.GetLanguageCode()
            };

            var projectCategories = _systemService.GetProjectCategories(filter);
            var projects = _systemService.GetProjects(filter);

            ViewBag.projectCategories = projectCategories;
            ViewBag.projects = projects;

            return View();
        }
    }
}