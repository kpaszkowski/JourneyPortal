using JourneyPortal.Models;
using JourneyPortal.ViewModels.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
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
            bool isExists = context.Users.Where(x => x.UserName == name).Any();
            if (!isExists)
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

        internal bool IsAtractionOwner(int id, string name)
        {
            var currentUser = userManager.FindByName(name);
            return context.Atractions.Where(x => x.Id == id && x.OwnerId == currentUser.Id).Any();
        }

        internal bool IsHotelOwner(int id, string name)
        {
            var currentUser = userManager.FindByName(name);
            return context.Hotels.Where(x => x.Id == id && x.OwnerId == currentUser.Id).Any();
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

        internal bool IsOwner(int offerId, string name)
        {
            var currentUser = userManager.FindByName(name);
            return context.Offers.Where(x => x.Id == offerId && x.TravelAgencyOwnerId == currentUser.Id).Any();
        }

        internal EditUserProfileViewModel PrepareEditUserProfile(string userName)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManagers = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    ApplicationUser currentUser = userManagers.FindByName(userName);

                    return context.Users.Where(x => x.Id == currentUser.Id).Select(x => new EditUserProfileViewModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        DateOfBirth = x.DateOfBirth,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool EditUserProfile(EditUserProfileViewModel model)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {


                    var user = context.Users.FirstOrDefault(x => x.Id == model.Id);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.DateOfBirth = model.DateOfBirth;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}