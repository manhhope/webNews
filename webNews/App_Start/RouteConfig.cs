using System.Web.Mvc;
using System.Web.Routing;
using webNews.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Configuration;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.OrmLite;
using webNews.Domain.Entities;
using webNews.Shared;

namespace webNews
{
    public class RouteConfig
    {
        public static IWebNewsDbConnectionFactory _connectionFactory;
        public RouteConfig(IWebNewsDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public static void RewirteUrl(RouteCollection routes)
        {
            _connectionFactory = new WebNewsDbConnectionFactory(ConfigurationManager.ConnectionStrings["WebNews"].ConnectionString, SqlServer2014OrmLiteDialectProvider.Instance);

            using (var db = _connectionFactory.OpenDbConnection())
            {
                var menus = db.Select<System_Menu>();
                var url = string.Empty;
                var localization = "{language}/";
                foreach (var menu in menus)
                {
                    if (!string.IsNullOrEmpty(menu.Slug) && menu.Area == "FE")
                    {
                        url = $"{menu.Slug}";
                    }
                    else if (!string.IsNullOrEmpty(menu.Slug) && menu.Area != "FE")
                    {
                        url = $"{menu.Area}/{menu.Slug}";
                    }
                    else if (menu.Area == "FE")
                    {
                        url = menu.Controller + "/" + menu.Action;
                    }
                    else if (menu.Area != "FE")
                    {
                        url = menu.Area + "/" + menu.Controller +"/" + menu.Action;
                    }


                    routes.MapRoute(
                      name: menu.Controller + "_" + menu.Lang,
                      url: url,
                      defaults: new
                      {
                          controller = menu.Controller,
                          action = menu.Action,
                          language = CultureHelper.GetDefaultCulture()
                      }
                    );

                    routes.MapRoute(
                      name: menu.Controller + "_" + menu.Lang + "_localization",
                      url: localization + url,
                      defaults: new {
                          controller = menu.Controller,
                          action = menu.Action,
                          language = CultureHelper.GetDefaultCulture()
                      }
                    );

                }
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            RewirteUrl(routes);

            //Map news detail
            routes.MapRoute(
                name: "NewsDetail",
                url: "{language}/tin-tuc-su-kien/{category_name}/{post_name}/{id}",
                defaults: new
                    {
                        controller = "News",
                        action = "Detail",
                        language = CultureHelper.GetDefaultCulture(),
                        category_name = UrlParameter.Optional,
                        post_name = UrlParameter.Optional,
                        id = UrlParameter.Optional
                    }
                );

            //Map news detail
            routes.MapRoute(
                name: "RecruitmentDetail",
                url: "{language}/tuyen-dung/{category_name}/{post_name}/{id}",
                defaults: new
                {
                    controller = "Recruitment",
                    action = "Detail",
                    language = CultureHelper.GetDefaultCulture(),
                    category_name = UrlParameter.Optional,
                    post_name = UrlParameter.Optional,
                    id = UrlParameter.Optional
                }
                );

            routes.MapRoute("DefaultLocalized",
            "{language}/{controller}/{action}/{id}",
            new
            {
                controller = "Home",
                action = "Index",
                id = "",
                language = CultureHelper.GetDefaultCulture()
            });

            routes.MapRoute(
                "Default",     // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home",
                    action = "Index",
                    id = "",
                    language = CultureHelper.GetDefaultCulture()
                }  // Parameter defaults
            );
        }
    }
}
