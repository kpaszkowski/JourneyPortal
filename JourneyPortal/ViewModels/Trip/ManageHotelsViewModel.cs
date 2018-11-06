using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace JourneyPortal.ViewModels.Trip
{
    public class ManageHotelsViewModel
    {
        public IPagedList<HotelGridViewModel> hotelList { get; set; }
        public int Page { get; set; }
    }
}