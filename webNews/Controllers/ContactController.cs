using System;
using System.Web.Mvc;
using webNews.Domain.Services;
using webNews.Security;

namespace webNews.Controllers
{
    public class ContactController : Controller
    {
        private readonly ISystemService _systemService;

        public ContactController(ISystemService systemService)
        {
            _systemService = systemService;
        }
        // GET: Contact
        public ActionResult Index()
        {


            return View();
        }
    }
}