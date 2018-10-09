using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Text")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać text")]
        public string Text { get; set; }
        public int CategoryId { get; set; }
    }
}