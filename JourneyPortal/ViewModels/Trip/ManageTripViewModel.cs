using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class ManageTripViewModel
    {
        public IPagedList<TripGridViewModel> tripList { get; set; }
    }
}