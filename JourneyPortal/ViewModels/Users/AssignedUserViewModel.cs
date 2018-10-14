using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Users
{
    public class AssignedUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Liczba zarezerwowanych miejsc")]
        public int NumberOfBooking { get; set; }

        [Display(Name = "Nazwa oferty")]
        public string OfferName { get; set; }

        public int OfferId { get; set; }
    }
}