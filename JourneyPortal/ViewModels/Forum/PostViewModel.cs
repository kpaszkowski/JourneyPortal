using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tekst")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Tekst nie może być pusty.")]
        public string Text { get; set; }
        [Display(Name = "Data utworzenia")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Like")]
        public int Likes { get; set; }
        [Display(Name = "Autor")]
        public string AuthorName { get; set; }
        [Display(Name = "Avatar")]
        public string AuthorAvatar { get; set; }
    }
}