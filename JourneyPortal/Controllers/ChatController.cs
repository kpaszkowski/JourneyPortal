using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        // GET: Chat
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}