using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class Atraction : TouristFacility
    {
        public string Type { get; set; }
        public decimal Cost { get; set; }
        public int TimeOfSightseeing { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}