using System;
using System.Web.Mvc;
using webNews.Domain.Entities;
using webNews.Domain.Services;
using webNews.Security;

namespace webNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISystemService _systemService;

        public HomeController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: /Home/
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

        public ActionResult ChangeLanguage(string lang)
        {
            Authentication.MarkLanguage(lang);

            return null;
        }

        public ActionResult SendEmail()
        {
            return null;
        }
	}
}