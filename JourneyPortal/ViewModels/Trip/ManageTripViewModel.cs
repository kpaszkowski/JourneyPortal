using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class ManageTripViewModel
    {
        public int Page { get; set; }
        public IPagedList<TripGridViewModel> tripList { get; set; }
    }
}