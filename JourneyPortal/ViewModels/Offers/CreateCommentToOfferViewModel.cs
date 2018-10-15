using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class CreateCommentToOfferViewModel
    {
        public int OfferId { get; set; }
        [Display(Name = "Tekst")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tekst jest wymagany!")]
        public string Text { get; set; }
        [Display(Name = "Ocena")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ocena jest wymagana!")]
        [Range(1, 5)]
        public int Rate { get; set; }
    }
}