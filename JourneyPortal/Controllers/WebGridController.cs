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
    [Authorize]
    public class WebGridController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public WebGridController()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }
        public ActionResult WebGrid()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            WebGridViewModel model = new WebGridViewModel();
            model.GetRoles(context);
            model.PageSize = 10;
            
            ViewBag.dropdown = new SelectList(context.Roles.ToList(), "Name", "Name");
            var users = context.Users.ToList();

            if (users != null)
            {
                model.TotalCount = users.Count();
                model.ListOfUser = users.Select((x, index) => new UserProfileInfo
                {
                    ID = index+1,
                    Login = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth,
                    Role = (from user in context.Users
                            where user.UserName == x.UserName
                            select new
                            {
                                RoleNames = (from userRole in user.Roles
                                             join role in context.Roles on userRole.RoleId
                                             equals role.Id
                                             select role.Name).ToList()
                            }).ToList().FirstOrDefault().RoleNames.FirstOrDefault(),
            });
            }
            return View("~/Views/WebGrid/UsersList.cshtml",model);
        }

        [HttpPost]
        public ActionResult ChangeRoleForUser(string userName , string newRole)
        {
            var currentUser = context.Users.FirstOrDefault(x => x.UserName == userName);
            var currentRole = (from user in context.Users
                             where user.UserName == userName
                             select new
                             {
                                 RoleNames = (from userRole in user.Roles
                                              join role in context.Roles on userRole.RoleId
                                              equals role.Id
                                              select role.Name).ToList()
                             }).ToList().FirstOrDefault().RoleNames.FirstOrDefault();

            if (currentRole != null)
            {
                var result = userManager.RemoveFromRole(currentUser.Id, currentRole);
            }
            var result2 = userManager.AddToRole(currentUser.Id, newRole);
            return RedirectToAction("WebGrid", "WebGrid");
        }
    }
}