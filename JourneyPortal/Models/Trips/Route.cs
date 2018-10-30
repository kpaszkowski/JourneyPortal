using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class Route
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public double StartX { get; set; }
        public double EndX { get; set; }
        public double StartY { get; set; }
        public double EndY { get; set; }

        public virtual Trip Trip { get; set; }
    }
}