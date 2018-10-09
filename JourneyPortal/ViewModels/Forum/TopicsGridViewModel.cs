using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class TopicsGridViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa tematu")]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Ostatnia Aktywność")]
        public DateTime LastActivity { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Wyświetlenia")]
        public int Views { get; set; }
    }
}