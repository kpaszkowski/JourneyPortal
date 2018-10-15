using JourneyPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tekst")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Tekst jest wymagany!")]
        public string Text { get; set; }
        [Display(Name = "Data uwtorzenia")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Autor")]
        public string AuthorName { get; set; }
        public string AuthorAvatar { get; set; }
        [Display(Name = "Ocena")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ocena jest wymagana!")]
        [Range(1,5)]
        public int Rate { get; set; }
    }
}