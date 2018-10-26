using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Chat
{
    public class ConversationViewModel
    {
        public int Id { get; set; }
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public string FistUserName { get; set; }
        public string SecondUserName { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public bool IsActive { get; set; }
    }
}