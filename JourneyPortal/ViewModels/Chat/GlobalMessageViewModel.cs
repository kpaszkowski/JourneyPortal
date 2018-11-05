using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Chat
{
    public class GlobalMessageViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tekst")]
        public string Text { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Autor")]
        public string AuthorName { get; set; }
    }
}