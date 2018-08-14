using System.Web.Mvc;
using System.Web.Routing;
using webNews.Domain;

namespace webNews
{
    public class RouteConfig
    {
        public static IWebNewsDbConnectionFactory _connectionFactory;
        //public RouteConfig(IWebNewsDbConnectionFactory connectionFactory)
        //{
        //    _connectionFactory = connectionFactory;
        //}

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

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
