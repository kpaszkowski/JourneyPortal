using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Others
{
    public class ManageConfigViewModel
    {
        public bool IsAdmin { get; set; }

        [Display(Name = "Limit kilometrów wyszukiwania atrakcji")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać wartość")]
        [Range(0.0, double.MaxValue)]
        public double KmLimit { get; set; }
        [Display(Name = "Ilość elementów na stronie")]
        [Required(AllowEmptyStrings = false , ErrorMessage = "Należy podać wartość")]
        [Range(0,int.MaxValue)]
        public int ItemPerPage { get; set; }
    }
}