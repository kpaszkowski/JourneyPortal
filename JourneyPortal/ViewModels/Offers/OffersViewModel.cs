using JourneyPortal.Helpers;
using JourneyPortal.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class OffersViewModel : BaseFormViewModel
    {
        public bool IsUser { get; set; }
        public bool IsTravelAgency { get; set; }
        public List<CreateOfferDetailViewModel> OffersList { get; set; }
    }
}