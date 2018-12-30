using JourneyPortal.Models;
using JourneyPortal.ViewModels.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static JourneyPortal.Enums.Enums;


namespace JourneyPortal.Services
{
    public class UserServices
    {
        internal string GetUserRole(string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
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

        internal List<UserListGridViewModel> GetAllUsers()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userList = context.Users.Select(x => new UserListGridViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        Role = (from user in context.Users
                                where user.UserName == x.UserName
                                select new
                                {
                                    RoleNames = (from userRole in user.Roles
                                                 join role in context.Roles on userRole.RoleId
                                                 equals role.Id
                                                 select role.Name).ToList()
                                }).ToList().FirstOrDefault().RoleNames.FirstOrDefault(),
                    }).ToList();
                    foreach (var item in userList)
                    {
                        if (item.Role == "TravelAgency")
                        {
                            item.Role = "Biuro podróży";
                        }
                        else if(item.Role == "Proprietor")
                        {
                            item.Role = "Właściciel atrakcji";
                        }
                        else if (item.Role == "User")
                        {
                            item.Role = "Uzytkownik";
                        }
                        else
                        {
                            item.Role = "Administrator";
                        }
                    }
                    return userList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<SelectListItem> GetAllRoles()
        {

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Administrator",
                Value = "Admin"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Użytkownik",
                Value = "User",
                Selected = true
            });
            listItems.Add(new SelectListItem
            {
                Text = "Właścieciel atrakcji",
                Value = "Proprietor"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Biuro podróży",
                Value = "TravelAgency"
            });

            return listItems;
        }

        internal void ChangeUserRole(string userId, string newRole)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = context.Users.FirstOrDefault(x => x.Id == userId);
                    var currentRole = (from user in context.Users
                                       where user.Id == userId
                                       select new
                                       {
                                           RoleNames = (from userRole in user.Roles
                                                        join role in context.Roles on userRole.RoleId
                                                        equals role.Id
                                                        select role.Name).ToList()
                                       }).ToList().FirstOrDefault().RoleNames.FirstOrDefault();

                    if (currentRole != null)
                    {
                        if (currentRole == "Admin")
                        {
                            if (GetAllUsers().Count(x => x.Role == "Administrator") == 1)
                            {
                                return;
                            }
                        }
                        var result = userManager.RemoveFromRole(currentUser.Id, currentRole);
                    }
                    var result2 = userManager.AddToRole(currentUser.Id, newRole);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(name);
                return context.Atractions.Where(x => x.Id == id && x.OwnerId == currentUser.Id).Any();
            }
        }

        internal bool IsHotelOwner(int id, string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(name);
                return context.Hotels.Where(x => x.Id == id && x.OwnerId == currentUser.Id).Any();
            }
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
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(name);
                if (currentUser == null)
                {
                    currentUser = new ApplicationUser();
                    currentUser.Id = "-1";
                }
                return context.Offers.Where(x => x.Id == offerId && x.TravelAgencyOwnerId == currentUser.Id).Any();
            }
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
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool IsTripOwner(int tripId, string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(name);
                return context.Trips.Where(x => x.Id == tripId && x.CreatedById == currentUser.Id).Any();
            }
        }
    }
}