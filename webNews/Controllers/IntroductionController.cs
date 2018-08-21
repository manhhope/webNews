using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Entities;
using webNews.Domain.Services;
using webNews.Security;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    public class IntroductionController : BaseController
    {
        private readonly ISystemService _systemService;

        public IntroductionController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: Introduction
        [GZipOrDeflate]
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
                Type = News.TYPE_INTRODUCTION,
                Lang = Authentication.GetLanguageCode()
            };

            var newsCategories = _systemService.GetNewCategories(filter);
            var news = _systemService.GetNews(filter);

            ViewBag.newsCategories = newsCategories;
            ViewBag.news = news;

            return View();
        }
        [GZipOrDeflate]
        [OutputCache(CacheProfile = "PageDetail")]
        public ActionResult Detail(int id)
        {
            var news = _systemService.GetNews(id, News.TYPE_INTRODUCTION);

            if (null == news)
                return RedirectToAction("Error", "Index");

            return View(news);
        }
    }
}