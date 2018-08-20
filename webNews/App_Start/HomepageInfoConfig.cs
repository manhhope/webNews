using ServiceStack.OrmLite.SqlServer;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using webNews.Domain;
using webNews.Domain.Repositories;
using webNews.Domain.Services;
using webNews.Security;

namespace webNews
{
    public class HomepageInfoConfig
    {
        public static ISystemService _systemService;
        public HomepageInfoConfig(ISystemService systemService)
        {
            _systemService = systemService;
        }

        public static void RewirteUrl(RouteCollection routes)
        {
            //_connectionFactory = new WebNewsDbConnectionFactory(ConfigurationManager.ConnectionStrings["WebNews"].ConnectionString, SqlServer2014OrmLiteDialectProvider.Instance);

            //using (var db = _connectionFactory.OpenDbConnection())
            //{
            //    var menus = db.Select<Menu>();
            //    foreach (var menu in menus)
            //    {
            //        if (!string.IsNullOrEmpty(menu.slug))
            //        {
            //            routes.MapRoute(
            //              name: menu.Controller,
            //              url: (string.IsNullOrEmpty(menu.Area) ? "" : menu.Area + "/") + menu.Slug,
            //              defaults: new { controller = menu.Controller, action = "Index", id = UrlParameter.Optional }
            //          ).DataTokens = new RouteValueDictionary(new { area = menu.Area });

            //        }
            //    }
            //}
        }

        public static void GetPageInfo()
        {
            IWebNewsDbConnectionFactory _connectionFactory = new WebNewsDbConnectionFactory(ConfigurationManager.ConnectionStrings["WebNews"].ConnectionString, SqlServer2014OrmLiteDialectProvider.Instance);
            ISystemRepository _systemRepository = new SystemRepository(_connectionFactory);

            if (Authentication.GetHomePageInfo() == null)
            {
                var homePageInfo = _systemRepository.GetPageInfo(new Models.Filter { Lang = Authentication.GetLanguageCode() });
                var partners = _systemRepository.GetMedias(new Models.Filter { ExtraInfo = Domain.Entities.Medium.TYPE_PARTER ,Lang = "all" });
                var banners = _systemRepository.GetMedias(new Models.Filter { ExtraInfo = Domain.Entities.Medium.TYPE_BANNER, Lang = "all" });
                var branches = _systemRepository.GetMedias(new Models.Filter { ExtraInfo = Domain.Entities.Medium.TYPE_BRANCH, Lang = "all" });
                var logo = _systemRepository.GetMedias(new Models.Filter { ExtraInfo = Domain.Entities.Medium.TYPE_LOGO, Lang = "all" });
                var menus = _systemRepository.GetMenu("FE");
                Authentication.MarkHomePageInfo(homePageInfo);
                Authentication.MarkPartners(partners);
                Authentication.MarkBanners(banners);
                Authentication.MarkBranches(branches);
                Authentication.MarkLogo(logo);
                Authentication.MarkMenuFE(menus);
            }
        }
    }
}
