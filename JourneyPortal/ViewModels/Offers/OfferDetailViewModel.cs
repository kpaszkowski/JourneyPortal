using JourneyPortal.Models;
using JourneyPortal.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class BookingFormViewModel :BaseFormViewModel
    {
        [Display(Name ="Liczba rezerwacji")]
        public int NumberOfBooking { get; set; }
        public int OfferId { get; set; }
    }
    public class CreateOfferDetailViewModel : BaseFormViewModel
    {
        [Display(Name = "Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać nazwę.")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis.")]
        public string Description { get; set; }

        [Display(Name = "Liczba miejsc")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać liczbę miejsc.")]
        public int NuberOfBooking { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę rozpoczęcia.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data zakończenia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę zakończenia.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Data stworzenia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę stworzenia.")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Koszt")]
        [DataType(DataType.Currency)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać koszt za osobę.")]

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Cost { get; set; }

        public byte[] Avatar { get; set; }

        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Display(Name = "Biuro podróży")]
        public string TravelAgencyOwnerName { get; set; }

    }

    public class OfferDetailViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać nazwę.")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis.")]
        public string Description { get; set; }

        [Display(Name = "Liczba miejsc")]
        [Range(1,999)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać liczbę miejsc.")]
        public int NuberOfBooking { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę rozpoczęcia.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data zakończenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać datę zakończenia.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Data stworzenia oferty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Koszt")]
        [DataType(DataType.Currency)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać koszt.")]
        public decimal Cost { get; set; }

        [Display(Name = "Kraj")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać kraj.")]
        public string Country { get; set; }

        [Display(Name = "Popularność")]
        public int Rate { get; set; }

        public List<CommentsViewModel> Comments { get; set; }

        public string Image { get; set; }

        [Display(Name = "Biuro podróży")]
        public string TravelAgencyOwnerName { get; set; }

        public bool IsUser { get; set; }
        public bool IsTravelAgency { get; set; }

        public bool IsActive { get; set; }
        public bool IsOwner { get; set; }
    }
}