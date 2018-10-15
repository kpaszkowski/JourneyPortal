using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class OffersGridViewModel
    {
        public int OfferId { get; set; }
        public IPagedList<OfferDetailViewModel> OfferDetailList { get; set; }

    }
}