using JourneyPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Services
{
    public class UserServices
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;
        public UserServices()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        internal ApplicationUser GetUserByName(string name)
        {

            return userManager.FindByName(name);
        }



        //public UserProfileInfo GetUserProfileInfo(string )
        //{

        //}
    }
}