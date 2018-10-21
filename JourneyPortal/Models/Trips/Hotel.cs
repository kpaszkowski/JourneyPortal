using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class Hotel : TouristFacility
    {
        public decimal CostPerNight { get; set; }
        public virtual ICollection<Trip> Trip { get; set; }
    }
}