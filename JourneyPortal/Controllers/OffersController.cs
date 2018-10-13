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
using PagedList;

namespace JourneyPortal.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        UserServices userServices;
        OfferServices offerServices;

        public OffersController()
        {
            userServices = new UserServices();
            offerServices = new OfferServices();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var cachedViewModel = new OffersViewModel();
            string currentUserName = User.Identity.Name;
            cachedViewModel.IsTravelAgency = userServices.IsTravelAgency(currentUserName);
            cachedViewModel.IsAdmin= userServices.IsAdmin(currentUserName);
            cachedViewModel.IsUser = userServices.IsUser(currentUserName);
            cachedViewModel.OffersList =  offerServices.GetAllOffers();
            return View("~/Views/Offers/Index.cshtml", cachedViewModel);
        }
        [HttpGet]
        public ActionResult CreateNewOffert()
        {
            var cachedViewModel = new CreateOfferDetailViewModel();
            return View("~/Views/Offers/CreateNewOffert.cshtml", cachedViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewOffert(CreateOfferDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool success = offerServices.CreateNewOffert(model,User.Identity.Name);
            }
            return RedirectToAction("Index", "Offers");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetYourOffers()
        {
            var model = offerServices.GetOffersForTravelAgency(User.Identity.Name);
            return View("~/Views/Offers/GetYourOffers.cshtml", model);

        }

        [HttpGet]
        public ViewResult GetOffersForUser(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var cachedViewModel = new List<OfferDetailViewModel>();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = userManager.FindByName(User.Identity.Name);

                var query = context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).OffersApplicationUsers.Select(x => x.Offers)
                    .Select(x => new OfferDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Cost = x.Cost,
                        TravelAgencyOwnerName = context.Users.FirstOrDefault(y => y.Id == x.TravelAgencyOwnerId).UserName,
                        NuberOfBooking = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id && y.OfferId == x.Id).BookingCount
                    });

                ViewBag.ItemNumber = query.Count();
                #region Sort Search

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(searchString.ToLower())
                                           || s.TravelAgencyOwnerName.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder == "idDesc" ? "Id" : "idDesc";
                ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
                ViewBag.DateStartSortParm = sortOrder == "startDate" ? "startDateDesc" : "startDate";
                ViewBag.DateEndSortParm = sortOrder == "endDate" ? "endDateDesc" : "endDate";
                ViewBag.BookingNumber = sortOrder == "bookingNumber" ? "bookingNumberDesc" : "bookingNumber";
                ViewBag.Cost = sortOrder == "cost" ? "costDesc" : "cost";
                ViewBag.TravelAgencyName = sortOrder == "travelAgencyName" ? "travelAgencyNameDesc" : "travelAgencyName";
                switch (sortOrder)
                {
                    case "Id":
                        query = query.OrderBy(s => s.Id);
                        break;
                    case "name":
                        query = query.OrderBy(s => s.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(s => s.Name);
                        break;
                    case "startDate":
                        query = query.OrderBy(s => s.StartDate);
                        break;
                    case "startDateDesc":
                        query = query.OrderByDescending(s => s.StartDate);
                        break;
                    case "endDate":
                        query = query.OrderBy(s => s.EndDate);
                        break;
                    case "endDateDesc":
                        query = query.OrderByDescending(s => s.EndDate);
                        break;
                    case "bookingNumber":
                        query = query.OrderBy(s => s.NuberOfBooking);
                        break;
                    case "bookingNumberDesc":
                        query = query.OrderByDescending(s => s.NuberOfBooking);
                        break;
                    case "cost":
                        query = query.OrderBy(s => s.Cost);
                        break;
                    case "costDesc":
                        query = query.OrderByDescending(s => s.Cost);
                        break;
                    case "travelAgencyName":
                        query = query.OrderBy(s => s.TravelAgencyOwnerName);
                        break;
                    case "travelAgencyNameDesc":
                        query = query.OrderByDescending(s => s.TravelAgencyOwnerName);
                        break;
                    default:
                        query = query.OrderByDescending(s => s.Id);
                        break;
                }

                #endregion Sort Search

                cachedViewModel = query.ToList();

            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageSize;
            return View("~/Views/Offers/GetAssignedOffers.cshtml", cachedViewModel.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult BookTrip(string sessionCacheKey ,int bookingCount, int offerId)
        {
            var cachedViewModel = SessionCache.Get<CreateOfferDetailViewModel>(sessionCacheKey);
            ViewBag.IsUser = userServices.IsUser(User.Identity.Name);
            if (bookingCount <1 || bookingCount > cachedViewModel.NuberOfBooking)
            {
                ModelState.AddModelError("bookingCount", "Podana liczba miejsc jest nieprawidłowa!");
                return View("~/Views/Offers/OfferDetailInfo.cshtml", cachedViewModel);
            }
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(User.Identity.Name);
                    var currentOffert = context.Offers.FirstOrDefault(x => x.Id == offerId);
                    //update
                    if (context.OffersApplicationUsers.Where(x => x.ApplicationUserId == currentUser.Id && x.OfferId == currentOffert.Id).Any())
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

            
            return RedirectToAction("GetOfferDetailInfo", "Offers");
        }

        [HttpPost]
        public ActionResult SignOff(int offerId, int? numberOfBooking)
        {
            if (numberOfBooking != null && numberOfBooking != 0)
            {
                var userName = User.Identity.Name;
                bool result = offerServices.SignOff(offerId, numberOfBooking,userName);
            }
            return RedirectToAction("GetOffersForUser");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOffersDetail(int id)
        {
            var model = offerServices.GetOfferDetail(id);
            string userName = User.Identity.Name;
            model.IsUser = userServices.IsUser(userName);
            model.IsTravelAgency= userServices.IsTravelAgency(userName);
            return View("~/Views/Offers/OfferDetailInfo.cshtml", model);
        }
    }
}