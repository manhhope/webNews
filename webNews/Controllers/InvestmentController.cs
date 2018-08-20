using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Entities;
using webNews.Domain.Services;
using webNews.Models;
using webNews.Security;

namespace webNews.Controllers
{
    public class InvestmentController : Controller
    {
        private readonly ISystemService _systemService;

        public InvestmentController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: Service
        public ActionResult Index()
        {
            var newsCategorieId = Convert.ToInt32(HttpContext.Request.Params.Get("cateId"));
            var page = Convert.ToInt32(HttpContext.Request.Params.Get("page"));

            var filter = new webNews.Models.Filter
            {
                Page = page - 1 < 0 ? 0 : page - 1,
                CateId = newsCategorieId,
                Type = News.TYPE_INVESTMENT,
                Lang = Authentication.GetLanguageCode()
            };

            var newsCategories = _systemService.GetNewCategories(filter);
            var news = _systemService.GetNews(filter);

            ViewBag.newsCategories = newsCategories;
            ViewBag.news = news;
            
            return View();
        }

        public ActionResult Detail()
        {

            var newsId = Convert.ToInt32(HttpContext.Request.Params.Get("id"));

            var news = _systemService.GetNews(newsId, News.TYPE_INVESTMENT);

            if (null == news)
                return RedirectToAction("Error", "Index");

            return View(news);
        }
    }
}