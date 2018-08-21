using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Entities;
using webNews.Domain.Services;
using webNews.Security;
using webNews.Shared;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISystemService _systemService;

        public HomeController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: /Home/
        [GZipOrDeflate]
        public ActionResult Index()
        {
            var filter = new webNews.Models.Filter
            {
                Page = 0,
                CateId = 0,
                Type = News.TYPE_NEWS,
                Lang = Authentication.GetLanguageCode(),
                PageLength = 15
            };

            var newsCategories = _systemService.GetNewCategories(filter);
            var news = _systemService.GetNews(filter);

            var projects = _systemService.GetProjects(filter);

            ViewBag.projects = projects;
            ViewBag.newsCategories = newsCategories;
            ViewBag.news = news;

            return View();
        }

        public ActionResult ChangeLanguage(string id)
        {
            // Validate input
            id = CultureHelper.GetImplementedCulture(id);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = id;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = id;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Authentication.MarkLanguage(id);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
        public ActionResult SetCulture(string id)
        {
            // Validate input
            id = CultureHelper.GetImplementedCulture(id);
            RouteData.Values["language"] = id;  // set culture


            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            return null;
        }
	}
}