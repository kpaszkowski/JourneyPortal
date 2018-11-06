using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class ManageAtractionsViewModel
    {
        public bool IsPreviewOnly { get; set; }
        public IPagedList<AtractionsGridViewModel> atractionsList { get; set; }
        public int Page { get; set; }
    }
}