using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Chat
{
    public class GlobalMessages
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}