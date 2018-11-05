using JourneyPortal.Services;
using JourneyPortal.ViewModels.Chat;
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
            model.messagesList = ChatService.GetAllMessages();
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

    }
}