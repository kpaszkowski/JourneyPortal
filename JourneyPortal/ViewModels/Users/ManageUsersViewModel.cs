using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.ViewModels.Users
{
    public class ManageUsersViewModel
    {
        public List<SelectListItem> RolesList { get; set; }
        public IPagedList<UserListGridViewModel> usersList { get; set; }
    }
}