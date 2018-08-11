using System;
using System.Net;
using System.Web.Mvc;
using NLog;
using ZP.AnPos.Web;

using Lang = webNews.Resources.Language;
namespace webNews.Controllers
{
    [Language]
    public class ErrorController : Controller
    {
        //
        private readonly Logger _log = LogManager.GetLogger("ErrorController");

        public ActionResult Index(int? errorCode = null, string errorMsg = default(string))
        {
            try
            {
                if (errorCode != null)
                {
                    ViewBag.errorCode = errorCode;
                }
                else
                {
                    ViewBag.errorCode = "500";
                }
                switch (errorCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        ViewBag.ErrorTitle = Lang.PagenotFound_Lang;//"Page Not Found!";
                        ViewBag.ErrorMsg = Lang.PageNotFoundMess_Lang; // "We're sorry, the page you requested cannot be found.";
                        return View();
                    case (int)HttpStatusCode.Unauthorized:
                        if (HttpContext.Session == null || !Request.IsAuthenticated)
                        {
                            RedirectToAction("Index", "Login");
                        }
                        ViewBag.ErrorTitle = Lang.Access_Denied_Lang; //"Access Denied!";
                        ViewBag.ErrorMsg = Lang.PermissionContent_Lang;
                        return View();

                    case (int)HttpStatusCode.Forbidden:
                        ViewBag.ErrorTitle = Lang.LockedAccount_Lang; //"Locked Account!";
                        ViewBag.ErrorMsg = Lang.LockedAccount_Lang;
                        return View();
                    default:
                        ViewBag.ErrorTitle = Lang.Error_Lang; // "Error!";
                        ViewBag.ErrorMsg = string.IsNullOrEmpty(errorMsg) ? Lang.Errororequest_Lang : errorMsg;
                        return View();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Error is ErrorController {0}", errorCode);
                ViewBag.ErrorTitle = Lang.Error_Lang; // "Error!";
                _log.ErrorException(errorMessage, ex);
                return View();
            }
        }
    }
}