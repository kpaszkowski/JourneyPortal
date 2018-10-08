using JourneyPortal.Helpers;
using JourneyPortal.Models;
using JourneyPortal.Models.Offer;
using JourneyPortal.ViewModels.Offers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    public class OffersController : Controller
    {
        public ActionResult Index()
        {
            var cachedViewModel = new OffersViewModel();
            SessionCache.Set(cachedViewModel);
            cachedViewModel.IsTravelAgency = isTravelAgencyRole();
            cachedViewModel.IsUser = isUserRole();
            cachedViewModel.OffersList = GetAllOffers();
            var a = new OfferDetail()
            {
                Name = "du[a",
                
            };
            cachedViewModel.OffersList.Add(a);
            return View("~/Views/Offers/Index.cshtml",cachedViewModel);
        }

        private List<OfferDetail> GetAllOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Offers.Select(x => new OfferDetail
                {
                    ID = x.Id,
                    Cost = x.Cost,
                    CreationDate = x.CreationDate,
                    Description = x.Description,
                    EndDate = x.EndDate,
                    Name = x.Name,
                    NuberOfBooking = x.NuberOfBooking,
                    StartDate = x.StartDate,
                }).ToList();
            }
            
        }

        [HttpGet]
        public ActionResult AddNewOffert()
        {
            return View("~/Views/Offers/AddNewOffert.cshtml");
        }

        [HttpGet]
        public ActionResult GetOffers()
        {
            return View("~/Views/Offers/GetOffers.cshtml");
        }
        public Boolean isUserRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Any())
                {
                    if (s[0].ToString() == "User" || s[0].ToString() == "Admin")
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
        public Boolean isTravelAgencyRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Any())
                {
                    if (s[0].ToString() == "TravelAgency" || s[0].ToString() == "Admin")
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