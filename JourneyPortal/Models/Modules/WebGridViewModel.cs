﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Modules
{
    public class WebGridViewModel
    {
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<UserProfileInfo> ListOfUser { get; set; }
        public int MyProperty { get; set; }
        public List<string> RolesDistionary { get; set; }

        internal void GetRoles(ApplicationDbContext context)
        {
            RolesDistionary = context.Roles.Select(x=>x.Name).ToList();
        }
    }
}