using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Offer
{
    public class OffersComment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int Like { get; set; }
        public ApplicationUser Author { get; set; }
    }
}