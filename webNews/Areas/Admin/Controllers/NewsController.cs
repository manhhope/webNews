using System;
using System.Collections.Generic;
using System.Web.Mvc;
using webNews.Domain;
using webNews.Language.Language;
using webNews.Areas.Admin.Models;
using webNews.Controllers;
using webNews.Domain.Services.RoleManage;
using NLog;
using webNews.Security;
using webNews.Domain.Entities;
using webNews.Domain.Services.News;
using webNews.Models.News;
using static webNews.FilterConfig;

namespace webNews.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        #region khởi tạo
        private readonly Logger _log = LogManager.GetLogger("NewsController");
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        #endregion

        #region trang index
        [GZipOrDeflate]
        public ActionResult Index()
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            if (!CheckAuthorizer.Authorize(Permission.VIEW))
                return RedirectToAction("Permission", "Error");
            return View();
        }
        #endregion

        #region tìm kiếm 
        [HttpPost]
        public ActionResult Search(NewsSearchModel model, int offset, int limit)
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            try
            {
                int pageIndex;

                if (offset == 0 || offset < limit)
                    pageIndex = 0;
                else
                    pageIndex = (offset / limit);

                var list = _newsService.GetListNews(model, pageIndex, limit);
                var total = 0;
                if (list == null)
                {
                    total = 0;
                    return Json(new
                    {
                        total
                    }, JsonRequestBehavior.AllowGet);
                }
                var data = list.DataList;

                total = list.Total;
                return Json(new
                {
                    data,
                    total
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error("GetData in NewsController error : " + ex);
                return null;
            }
        }

        #endregion
    }
}
