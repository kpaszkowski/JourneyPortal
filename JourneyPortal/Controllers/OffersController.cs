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
            SessionCache.Set(cachedViewModel);
            cachedViewModel.IsTravelAgency = isTravelAgencyRole();
            cachedViewModel.IsUser = isUserRole();
            cachedViewModel.OffersList = GetAllOffers();
            return View("~/Views/Offers/Index.cshtml", cachedViewModel);
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
        public ActionResult BookTrip(int bookingCount, int offerId)
        {
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

            return RedirectToAction("Index", "Offers");
        }

        [HttpPost]
        public ActionResult SignOff(int offerId, int? numberOfBooking)
        {
            if (numberOfBooking != null && numberOfBooking != 0)
            {
                var userName = User.Identity.Name;
                bool result = offerServices.SignOff(offerId, numberOfBooking,userName);
            }
            return RedirectToAction("GetAssignedOffers");
        }

        [HttpGet]
        public ViewResult GetAssignedOffers(string sortOrder, string currentFilter, string searchString, int? page)
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

        [HttpGet]
        public ActionResult CreateNewOffert()
        {
            var cachedViewModel = new CreateOfferDetailViewModel();

            SessionCache.Set(cachedViewModel);
            return View("~/Views/Offers/CreateNewOffert.cshtml", cachedViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
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
                List<CreateOfferDetailViewModel> listWithOffers = context.Offers.Where(x => x.TravelAgencyOwner.UserName == User.Identity.Name).Select(x => new CreateOfferDetailViewModel
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

                return View("~/Views/Offers/GetYourOffers.cshtml", listWithOffers);
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOfferDetailInfo(int id, string parentSessionCacheKey)
        {
            var mainViewModel = SessionCache.Get<OffersViewModel>(parentSessionCacheKey);

            var cachedViewModel = new CreateOfferDetailViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                cachedViewModel = context.Offers.Where(x => x.Id == id).Select(x => new CreateOfferDetailViewModel
                {
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
                return View("~/Views/Offers/OfferDetailInfo.cshtml", cachedViewModel);
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