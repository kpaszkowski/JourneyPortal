using JourneyPortal.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class MapViewModel
    {
        public bool IsProprietor { get; set; }
        public bool IsUser { get; set; }
        public bool DrawMap { get; set; }

        public List<AtractionsGridViewModel> Atractions { get; set; }
        public HotelGridViewModel Hotel { get; set; }
        public List<RouteViewModel> Routes { get; set; }
    }
}