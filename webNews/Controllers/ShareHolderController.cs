using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNews.Domain.Entities;
using webNews.Domain.Services;
using webNews.Models;
using webNews.Security;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    public class ShareHolderController : BaseController
    {
        private readonly ISystemService _systemService;

        public ShareHolderController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: ShareHolder
        [GZipOrDeflate]
        public ActionResult Index()
        {
            var newsCategorieId = Convert.ToInt32(HttpContext.Request.Params.Get("cateId"));
            var page = Convert.ToInt32(HttpContext.Request.Params.Get("page"));

            var filter = new webNews.Models.Filter
            {
                Page = page - 1 < 0 ? 0 : page - 1,
                CateId = newsCategorieId,
                Type = News.TYPE_SHAREHOLDER,
                Lang = Authentication.GetLanguageCode()
            };

            var newsCategories = _systemService.GetNewCategories(filter);
            var news = _systemService.GetNews(filter);

            ViewBag.newsCategories = newsCategories;
            ViewBag.news = news;

            if (null == newsCategories)
                return RedirectToAction("Index", "Error");

            if (newsCategorieId > 0)
                return View("Category");

            return View();
        }

        [GZipOrDeflate]
        [OutputCache(CacheProfile = "PageDetail")]
        public ActionResult Detail(int id)
        {
            var news = _systemService.GetNews(id, News.TYPE_SHAREHOLDER);

            if (null == news)
                return RedirectToAction("Error", "Index");

            return View(news);
        }
    }
}