using JourneyPortal.Models;
using JourneyPortal.Models.Modules;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    public class WebGridController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;

        public WebGridController()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }
        public ActionResult WebGrid()
        {

            WebGridViewModel model = new WebGridViewModel();
            model.PageSize = 2;

            var users = context.Users.ToList();

            if (users != null)
            {
                model.TotalCount = users.Count();
                model.ListOfUser = users;
            }
            return View("~/Views/WebGrid/UsersList.cshtml",model);
        }
    }
}