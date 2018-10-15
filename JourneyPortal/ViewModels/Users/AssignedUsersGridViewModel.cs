using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Users
{
    public class AssignedUsersGridViewModel
    {
        public int OfferId { get; set; }
        public IPagedList<AssignedUserViewModel> AssignedUserList { get; set; }
    }
}