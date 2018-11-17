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
        public double KmLimit { get; set; }
        [Display(Name = "Ilość elementów na stronie")]
        public int ItemPerPage { get; set; }
    }
}