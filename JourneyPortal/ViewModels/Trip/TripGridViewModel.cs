using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class TripGridViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Długość w Km")]
        public double TotalDistance { get; set; }
        [Display(Name = "Czas trwania")]
        public int Duration { get; set; }
        [Display(Name = "Czas trwania przy dużym nateżeniu ruchu")]
        public int DurationTrafiic { get; set; }
        [Display(Name = "Liczba atrakcji")]
        public int AtractionNumber { get; set; }
    }
}