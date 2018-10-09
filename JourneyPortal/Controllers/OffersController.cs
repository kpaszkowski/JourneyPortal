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
    [Authorize]
    public class OffersController : Controller
    {
        UserServices userServices;

        public OffersController()
        {
            userServices = new UserServices();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var cachedViewModel = new OffersViewModel();
            SessionCache.Set(cachedViewModel);
            cachedViewModel.IsTravelAgency = isTravelAgencyRole();
            cachedViewModel.IsUser = isUserRole();
            cachedViewModel.OffersList = GetAllOffers();
            return View("~/Views/Offers/Index.cshtml",cachedViewModel);
        }
        [AllowAnonymous]
        private List<CreateOfferDetailViewModel> GetAllOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Offers.Select(x => new CreateOfferDetailViewModel
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
        public ActionResult BookTrip(int bookingCount , int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(User.Identity.Name);
                    var currentOffert = context.Offers.FirstOrDefault(x => x.Id == offerId);
                    //update
                    if (context.OffersApplicationUsers.Where(x=>x.ApplicationUserId == currentUser.Id && x.OfferId == currentOffert.Id).Any())
                    {
                        var offerApplicationUser = context.OffersApplicationUsers.FirstOrDefault(x => x.ApplicationUserId == currentUser.Id && x.OfferId == currentOffert.Id);
                        offerApplicationUser.BookingCount += bookingCount;
                    }
                    //new
                    else
                    {
                        var offerApplicationUser = new OffersApplicationUsers
                        {
                            ApplicationUser = currentUser,
                            Offers = currentOffert,
                            BookingCount = bookingCount,
                        };
                        context.OffersApplicationUsers.Add(offerApplicationUser);
                    }
                    currentOffert.NuberOfBooking -= bookingCount;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return RedirectToAction("Index", "Offers");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAssignedOffers()
        {
            var cachedViewModel = new List<OfferDetailViewModel>();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                cachedViewModel = context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).OffersApplicationUsers.Select(x => x.Offers)
                    .Select(x => new OfferDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Cost = x.Cost,
                        TravelAgencyOwnerName = context.Users.FirstOrDefault(y => y.Id == x.TravelAgencyOwnerId).UserName,
                    }).ToList();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = userManager.FindByName(User.Identity.Name);
                cachedViewModel.ForEach(x => x.NuberOfBooking = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id
                   && y.OfferId == x.Id).BookingCount);

            }

            return View("~/Views/Offers/GetAssignedOffers.cshtml",cachedViewModel);
        }

        [HttpGet]
        public ActionResult CreateNewOffert()
        {
            var cachedViewModel = new CreateOfferDetailViewModel();

            SessionCache.Set(cachedViewModel);
            return View("~/Views/Offers/CreateNewOffert.cshtml",cachedViewModel);
        }

        [HttpPost]
        public ActionResult CreateNewOffert(CreateOfferDetailViewModel model)
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
                        Cost = model.Cost,
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
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                List<CreateOfferDetailViewModel> listWithOffers = context.Offers.Where(x=>x.TravelAgencyOwner.UserName == User.Identity.Name).Select(x => new CreateOfferDetailViewModel
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

                return View("~/Views/Offers/GetYourOffers.cshtml",listWithOffers);
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOfferDetailInfo(int id , string parentSessionCacheKey)
        {
            var mainViewModel = SessionCache.Get<OffersViewModel>(parentSessionCacheKey);

            var cachedViewModel = new CreateOfferDetailViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                cachedViewModel = context.Offers.Where(x => x.Id == id).Select(x => new CreateOfferDetailViewModel {
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