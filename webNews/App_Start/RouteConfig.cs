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
                      defaults: new { controller = menu.Controller, action = menu.Action}
                    );
                }
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            RewirteUrl(routes);

            //Map route news
            routes.MapRoute(
                name: "NewsCustom",
                url: "{post_name}/{id}",
                defaults: new
                {
                    controller = "News",
                    action = "Index",
                    post_name = UrlParameter.Optional,
                    id = UrlParameter.Optional
                }
            );

            //Map news detail
            routes.MapRoute(
                name: "NewsDetail",
                url: "tin-tuc/{category_name}/{post_name}/{id}",
                defaults: new
                    {
                        controller = "News",
                        action = "Detail",
                        category_name = UrlParameter.Optional,
                        post_name = UrlParameter.Optional,
                        id = UrlParameter.Optional
                    }
                );

        }
    }
}
