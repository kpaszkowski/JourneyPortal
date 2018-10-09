using JourneyPortal.Models;
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
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            if (!isAdminUser())
            {
                return RedirectToAction("Index", "Home");
            }
            ApplicationDbContext context = new ApplicationDbContext();
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        public ActionResult Delete(string Id)
        {
            if (!isAdminUser())
            {
                return RedirectToAction("Index", "Role");
            }
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleToRemove = context.Roles.FirstOrDefault(x => x.Id == Id);

            if (roleManager.RoleExists(roleToRemove.Name))
            {

                roleManager.Delete(roleToRemove);
            }

            return RedirectToAction("Index", "Role");

        }



        [HttpPost]
        public ActionResult Create(UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(model.UserRole))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = model.UserRole;
                roleManager.Create(role);
                ViewBag.Message = string.Format("Pomyślnie stworzono rolę {0}", model.UserRole);
            }
            else
            {
                ViewBag.ValidationMessage = string.Format("Rola {0} już istnieje", model.UserRole);
            }

            return View();
        }

        private bool isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}