using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CategoryGridViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }
        [Display(Name = "Ostatnia Aktywność")]
        public DateTime LastActivity { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}