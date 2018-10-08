﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Offer
{
    public class Offers
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Nazwa")]
        [Required(AllowEmptyStrings = false , ErrorMessage = "Należy podać nazwę.")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis.")]
        public string Description { get; set; }

        [Display(Name = "Liczba miejsc")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać liczbę miejsc.")]
        public int NuberOfBooking { get; set; }

        [Display(Name = "Biuro podróży")]
        public ApplicationUser TravelAgencyOwner { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę rozpoczęcia.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data zakończenia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę zakończenia.")]
        public DateTime EndDate { get; set; }

        [Display(Name= "Koszt")]
        [DataType(DataType.Currency)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać koszt za osobę.")]
        public decimal Cost { get; set; }

        [Display(Name = "Przypisani użytkownicy")]
        public virtual ICollection<ApplicationUser> AssignedUsers { get; set; }
    }
}