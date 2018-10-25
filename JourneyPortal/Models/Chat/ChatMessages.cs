using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Chat
{
    public class ChatMessages
    {
        [Key]
        public int Id { get; set; }
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public ApplicationUser FirstUser { get; set; }
        public ApplicationUser SecondUser { get; set; }

    }
}