using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using webNews.Shared;

namespace webNews.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        //Get from cookies
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    string cultureName = null;

        //    // Attempt to read the culture cookie from Request
        //    HttpCookie cultureCookie = Request.Cookies["_culture"];
        //    if (cultureCookie != null)
        //        cultureName = cultureCookie.Value;
        //    else
        //        cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
        //                Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
        //                null;
        //    // Validate culture name
        //    cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

        //    // Modify current thread's cultures            
        //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
        //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //    return base.BeginExecuteCore(callback, state);
        //}

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = RouteData.Values["language"] as string;

            // Attempt to read the culture cookie from Request
            if (cultureName == null)
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            if (RouteData.Values["language"] as string != cultureName)
            {
                // Force a valid culture in the URL
                RouteData.Values["language"] = cultureName.ToLowerInvariant(); // lower case too

                // Redirect user
                //Response.RedirectToRoute(RouteData.Values);
            }


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            return base.BeginExecuteCore(callback, state);
        }
    }
}