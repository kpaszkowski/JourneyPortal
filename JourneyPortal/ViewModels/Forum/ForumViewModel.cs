using JourneyPortal.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class ForumViewModel :BaseFormViewModel
    {
        public List<CategoryGridViewModel> Categories { get; set; }
        public bool IsAdmin { get; set; }
    }
}