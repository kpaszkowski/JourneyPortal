using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Modules
{
    public class WebGridViewModel
    {
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<ApplicationUser> ListOfUser { get; set; }
    }
}