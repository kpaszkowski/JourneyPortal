using JourneyPortal.Helpers;
using JourneyPortal.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace JourneyPortal.ViewModels.Offers
{
    public class OffersViewModel : BaseFormViewModel
    {
        public bool IsUser { get; set; }
        public bool IsTravelAgency { get; set; }

        public bool IsAdmin { get; set; }
        public IPagedList<OfferDetailViewModel> OffersList { get; set; }
    }
}