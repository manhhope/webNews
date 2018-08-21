using System;
using System.Web.Mvc;
using webNews.Domain.Services;
using webNews.Security;
using static webNews.FilterConfig;

namespace webNews.Controllers
{
    [OutputCache(CacheProfile = "StaticPage")]
    public class ContactController : BaseController
    {
        private readonly ISystemService _systemService;

        public ContactController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        // GET: Contact
        [GZipOrDeflate]
        public ActionResult Index()
        {
            return View();
        }
    }
}