using JourneyPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static JourneyPortal.Enums.Enums;

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

        internal string GetUserRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            return (from user in context.Users
                    where user.UserName == name
                    select new
                    {
                        RoleNames = (from userRole in user.Roles
                                     join role in context.Roles on userRole.RoleId
                                     equals role.Id
                                     select role.Name).ToList()
                    }).ToList().FirstOrDefault().RoleNames.FirstOrDefault();
        }
        
        internal bool IsTravelAgency(string name)
        {
            if (IsAdmin(name))
            {
                return true;
            }
            if (GetUserRole(name) == Roles.TravelAgency.ToString())
            {
                return true;
            }
            return false;
        }
        internal bool IsAdmin(string name)
        {
            if (GetUserRole(name) == Roles.Admin.ToString())
            {
                return true;
            }
            return false;
        }
        internal bool IsUser(string name)
        {
            if (IsAdmin(name))
            {
                return true;
            }
            if (GetUserRole(name) == Roles.User.ToString())
            {
                return true;
            }
            return false;
        }
        internal bool IsProprietor(string name)
        {
            if (IsAdmin(name))
            {
                return true;
            }
            if (GetUserRole(name) == Roles.Proprietor.ToString())
            {
                return true;
            }
            return false;
        }
    }
}