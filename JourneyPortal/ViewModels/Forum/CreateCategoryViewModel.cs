using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}