using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Atraction> Atractions { get; set; }

        public virtual ICollection<Route> Routes { get; set; }

        public int BaseHotelId { get; set; }
        public Hotel BaseHotel { get; set; }

        public string CreatedById{ get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public double TotalDistance { get; set; }
        public int Duration { get; set; }
        public int DurationTraffic { get; set; }
    }
}