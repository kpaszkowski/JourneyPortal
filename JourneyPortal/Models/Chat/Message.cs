using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Chat
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreationDate { get; set; }
        public Conversation Conversation { get; set; }
    }
}