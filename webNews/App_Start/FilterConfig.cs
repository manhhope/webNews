using System;
using System.Globalization;
using System.IO.Compression;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using webNews.Shared;

namespace webNews
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCache());
            filters.Add(new GZipOrDeflateAttribute());
            filters.Add(new InternationalizationAttribute());
        }

        public class LanguageAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var controller = filterContext.Controller;
                if (controller != null)
                    controller.ViewBag.Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }
        public class NoCache : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }

        public class GZipOrDeflateAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting
                 (ActionExecutingContext filterContext)
            {
                string acceptencoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];

                if (!string.IsNullOrEmpty(acceptencoding))
                {
                    acceptencoding = acceptencoding.ToLower();
                    var response = filterContext.HttpContext.Response;
                    if (acceptencoding.Contains("gzip"))
                    {
                        response.AppendHeader("Content-Encoding", "gzip");
                        response.Filter = new GZipStream(response.Filter,
                                              CompressionMode.Compress);
                    }
                    else if (acceptencoding.Contains("deflate"))
                    {
                        response.AppendHeader("Content-Encoding", "deflate");
                        response.Filter = new DeflateStream(response.Filter,
                                          CompressionMode.Compress);
                    }
                }
            }
        }

        public class InternationalizationAttribute : ActionFilterAttribute
        {

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                string language = (string)filterContext.RouteData.Values["language"] ?? "vi";
                string cultureName = CultureHelper.GetImplementedCulture(language); // This is safe

                if(language != "en")
                {
                    filterContext.RouteData.Values["language"] = "en";
                }

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

            }
        }
    }
}
