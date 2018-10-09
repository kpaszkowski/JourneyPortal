using JourneyPortal.Helpers;
using JourneyPortal.Models;
using JourneyPortal.Models.Offer;
using JourneyPortal.Services;
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
        UserServices userServices;

        public OffersController()
        {
            userServices = new UserServices();
        }

        public ActionResult Index()
        {
            var cachedViewModel = new OffersViewModel();
            SessionCache.Set(cachedViewModel);
            cachedViewModel.IsTravelAgency = isTravelAgencyRole();
            cachedViewModel.IsUser = isUserRole();
            cachedViewModel.OffersList = GetAllOffers();
            return View("~/Views/Offers/Index.cshtml",cachedViewModel);
        }

        private List<OfferDetailViewModel> GetAllOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                return context.Offers.Select(x => new OfferDetailViewModel
                {
                    ID = x.Id,
                    Cost = x.Cost,
                    CreationDate = x.CreationDate,
                    Description = x.Description,
                    EndDate = x.EndDate,
                    Name = x.Name,
                    NuberOfBooking = x.NuberOfBooking,
                    StartDate = x.StartDate,
                    TravelAgencyOwner = x.TravelAgencyOwner
                }).ToList();
            }
            
        }

        [HttpPost]
        public ActionResult BookTrip(int bookingCount)
        {
            
            return RedirectToAction("Index", "Offers");
        }

        [HttpGet]
        public ActionResult CreateNewOffert()
        {
            var cachedViewModel = new OfferDetailViewModel();

            SessionCache.Set(cachedViewModel);
            return View("~/Views/Offers/CreateNewOffert.cshtml",cachedViewModel);
        }

        [HttpPost]
        public ActionResult CreateNewOffert(OfferDetailViewModel model)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    Offers newOffert = new Offers()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        NuberOfBooking = model.NuberOfBooking,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        CreationDate = DateTime.Now,
                        TravelAgencyOwner = userManager.FindByName(User.Identity.Name),
                    };
                    context.Offers.Add(newOffert);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index", "Offers");
        }

        [HttpGet]
        public ActionResult GetOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                List<OfferDetailViewModel> listWithOffers = context.Offers.Where(x=>x.TravelAgencyOwner.UserName == User.Identity.Name).Select(x => new OfferDetailViewModel
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

                return View("~/Views/Offers/GetOffers.cshtml",listWithOffers);
            }

        }

        [HttpGet]
        public ActionResult GetOfferDetailInfo(int id , string parentSessionCacheKey)
        {
            var mainViewModel = SessionCache.Get<OffersViewModel>(parentSessionCacheKey);

            var cachedViewModel = new OfferDetailViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                cachedViewModel = context.Offers.Where(x => x.Id == id).Select(x => new OfferDetailViewModel {
                    ID = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreationDate = x.CreationDate,
                    NuberOfBooking = x.NuberOfBooking,
                    TravelAgencyOwner = x.TravelAgencyOwner,
                    Cost = x.Cost,
                    
                }).FirstOrDefault();
                ViewBag.IsUser = mainViewModel.IsUser;
                ViewBag.IsTravelAgency = mainViewModel.IsTravelAgency;
                return View("~/Views/Offers/OfferDetailInfo.cshtml",cachedViewModel);
            }
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