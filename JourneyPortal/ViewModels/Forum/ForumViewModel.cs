using JourneyPortal.ViewModels.Shared;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class ForumViewModel :BaseFormViewModel
    {
        public IPagedList<CategoryGridViewModel> Categories { get; set; }
        public bool IsAdmin { get; set; }
    }
}