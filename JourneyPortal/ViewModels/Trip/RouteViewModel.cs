using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public double StartX { get; set; }
        public double EndX { get; set; }
        public double StarY { get; set; }
        public double EndY { get; set; }
    }
}