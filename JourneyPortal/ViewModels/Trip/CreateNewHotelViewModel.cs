using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class CreateNewHotelViewModel
    {
        [Display(Name = "Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać nazwę.")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis.")]
        public string Description { get; set; }
        [Display(Name = "Koszt")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać koszt.")]
        public decimal CostPerNight { get; set; }
        [Display(Name = "X")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać współrzędne X.")]
        public double X { get; set; }
        [Display(Name = "Y")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać współrzędne Y.")]
        public double Y { get; set; }
    }
}