﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Trip
{
    public class ManageAtractionsViewModel
    {
        public IPagedList<AtractionsGridViewModel> atractionsList { get; set; }
    }
}