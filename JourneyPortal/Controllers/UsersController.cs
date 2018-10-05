using JourneyPortal.Models;
using JourneyPortal.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    public class UsersController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;

        public UsersController()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }
        // GET: Users
        public ActionResult Index()
        {
            var cachedViewModel = new UserProfileInfo();
            if (User.Identity.IsAuthenticated)
            {
                SessionCache.Set(cachedViewModel);
                var u = userManager.FindByName(User.Identity.Name.ToString());
                cachedViewModel.UpdateFromModel(u);

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return PartialView("~/Views/Users/Index.cshtml",cachedViewModel);
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return PartialView("~/Views/Users/Index.cshtml",cachedViewModel);
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Any())
                {
                    if (s[0].ToString() == "Admin")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}