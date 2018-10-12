using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Offer
{
    public class OffersApplicationUsers
    {
        [Key, Column(Order = 0)]
        public int OfferId { get; set; }
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }
        
        public int BookingCount { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Offers Offers { get; set; }

    }
    public class Offers
    {
        public Offers()
        {
            this.OffersApplicationUsers = new HashSet<OffersApplicationUsers>();
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int NuberOfBooking { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public int Rate { get; set; }

        public string Country { get; set; }

        public string TravelAgencyOwnerId { get; set; }

        public ApplicationUser TravelAgencyOwner { get; set; }

        public virtual ICollection<OffersApplicationUsers> OffersApplicationUsers { get; set; }
    }
}