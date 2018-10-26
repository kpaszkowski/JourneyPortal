using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Chat
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public ApplicationUser FirstUser { get; set; }
        public ApplicationUser SecondUser { get; set; }

    }
}