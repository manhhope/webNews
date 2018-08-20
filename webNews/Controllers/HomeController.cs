using System.Web.Mvc;
using webNews.Security;

namespace webNews.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login", new { Area = "Admin" });
            else
                return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Authentication.MarkLanguage(lang);

            return null;
        }

        public ActionResult SendEmail()
        {
            return null;
        }
	}
}