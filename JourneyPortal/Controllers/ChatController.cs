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
        public ChatServices chatService { get; set; }

        public ChatController()
        {
            chatService = new ChatServices();
        }

        public ActionResult Index()
        {
            var model = new ChatViewModel();
            model.userList = chatService.GetUsers(User.Identity.Name);
            return View("~/Views/Chat/Index.cshtml",model);
        }


        [HttpPost]
        public ActionResult StartConversation(string userId1 , string userId2)
        {
            var model = new ConversationViewModel();
            model = chatService.StartConversation(userId1, userId2);
            return View("~/Views/Chat/Conversation.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetConversation(string userId)
        {
            var model = new ConversationViewModel();
            model = chatService.GetConversation(userId, User.Identity.Name);
            return View("~/Views/Chat/Conversation.cshtml", model);
        }
    }
}