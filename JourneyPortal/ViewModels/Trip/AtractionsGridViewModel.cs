using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class AtractionsGridViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Popularność")]
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Email kontaktowy")]
        public string OwnerEmail { get; set; }
        [Display(Name = "Typ")]
        public string Type { get; set; }
        [Display(Name = "Koszt")]
        public decimal Cost { get; set; }
        [Display(Name = "Czas zwiedzania")]
        public int TimeOfSightseeing { get; set; }
        [Display(Name = "X")]
        public double X { get; set; }
        [Display(Name = "Y")]
        public double Y { get; set; }
    }
}