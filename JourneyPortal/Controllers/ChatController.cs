using JourneyPortal.Services;
using JourneyPortal.ViewModels.Chat;
using Newtonsoft.Json;
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
        public ChatService ChatService { get; set; }
        public ChatController()
        {
            ChatService = new ChatService();
        }

        public ActionResult Index()
        {
            var model = new ChatViewModel();
            return View("~/Views/Chat/Index.cshtml",model);
        }

        public ActionResult CreateNewMessage(GlobalMessageViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Text))
            {
                ChatService.CreateNewMessage(model.Text, User.Identity.Name);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetMessages()
        {

            return new JsonResult
            {
                Data = new
                {
                    messages = ChatService.GetAllMessages(),
                    success = true,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}