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
using JourneyPortal.ViewModels.Users;
using System.IO;
using System.Configuration;

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
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = 1;
            cachedViewModel.OffersList =  offerServices.GetAllOffers().ToPagedList(pageNumber,pageSize);
            return View("~/Views/Offers/Index.cshtml", cachedViewModel);
        }

        [AllowAnonymous]
        public ActionResult IndexGetAllOffers(int? page)
        {
            var cachedViewModel = new OffersViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            cachedViewModel.OffersList = offerServices.GetAllOffers().ToPagedList(pageNumber, pageSize);
            return PartialView("~/Views/Offers/Index.cshtml", cachedViewModel);
        }

        [HttpGet]
        public ActionResult CreateNewOffert()
        {
            var cachedViewModel = new CreateOfferDetailViewModel();
            return View("~/Views/Offers/CreateNewOffert.cshtml", cachedViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewOffert(CreateOfferDetailViewModel model, HttpPostedFileBase file)
        {
            if (model.StartDate > model.EndDate || model.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Wprowadź odpowiednią datę");
            }
            if (ModelState.IsValid)
            {
                bool success = offerServices.CreateNewOffert(model,User.Identity.Name,file,this);
                return RedirectToAction("GetYourOffers");
            }
            return View("~/Views/Offers/CreateNewOffert.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetYourOffers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var cachedViewModel = new OffersGridViewModel();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = userManager.FindByName(User.Identity.Name);
                 var query = currentUser.OwnerOffers.Select(x => new OfferDetailViewModel
                     {
                         Id = x.Id,
                         Name = x.Name,
                         StartDate = x.StartDate,
                         EndDate = x.EndDate,
                         Cost = x.Cost,
                         Rate = x.Rate,
                         Country = x.Country,
                         NuberOfBooking = x.NuberOfBooking,
                         IsActive = x.IsActive,
                     });

                #region Sort Search

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.ItemNumber = query.Count();
                ViewBag.CurrentFilter = searchString;

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(searchString.ToLower())
                                           || s.Country.ToLower().Contains(searchString.ToLower()));
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

                int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
                int pageNumber = (page ?? 1);
                ViewBag.PageSize = pageSize;
                var tempList = query.ToList();
                cachedViewModel.OfferDetailList = tempList.ToPagedList(pageNumber, pageSize);
            }

            return View("~/Views/Offers/GetYourOffers.cshtml", cachedViewModel);

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
                        NuberOfBooking = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id && y.OfferId == x.Id).BookingCount,
                        Status = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id && y.OfferId == x.Id).Status,
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

            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageSize;
            return View("~/Views/Offers/GetAssignedOffers.cshtml", cachedViewModel.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult EditImage(HttpPostedFileBase file, int offerId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var currentOffer = context.Offers.FirstOrDefault(x => x.Id == offerId);

                Guid id = Guid.NewGuid();
                if (file != null)
                {
                    Image image = new Image();
                    var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                    image.ImageUrl = file.ToString();
                    image.Name = currentOffer.Name + id + "-image";
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + image.Name + ext;
                        var path = Path.Combine("~/Content/OffersImages", myfile);
                        image.ImageUrl = path;
                        context.Images.Add(image);
                        file.SaveAs(Server.MapPath(path));
                    }
                    currentOffer.Image = image.ImageUrl;
                }
                else
                {
                    var image = context.Images.FirstOrDefault(x => x.ImageUrl == currentOffer.Image);
                    context.Images.Remove(image);
                    currentOffer.Image = null;
                }
                context.SaveChanges();
            }
            return RedirectToAction("GetOffersDetail", "Offers", new { id = offerId });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetComments(int offerId, int? page)
        {
            var cachedViewModel = new ComentsGridViewModel();

            cachedViewModel.OfferId = offerId;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                var query = context.OffersComments.Where(x => x.Offers.Id == offerId).Select(x=> new CommentsViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    CreationDate = x.CreationDate,
                    Rate = x.Rate
                });
                ViewBag.ItemNumber = query.Count();
                int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
                int pageNumber = (page ?? 1);
                ViewBag.PageSize = pageSize;
                var tempList = query.ToList();
                cachedViewModel.CommentsList = tempList.ToPagedList(pageNumber, pageSize);
                ViewBag.ItemNumber = cachedViewModel.CommentsList.Count();
            }

            return PartialView("~/Views/Offers/CommentsGrid.cshtml", cachedViewModel);
        }

        [HttpGet]
        public ActionResult GetAssignedUsers(int offerId ,string sortOrder, string currentFilter, string searchString, int? page)
        {
            var cachedViewModel = new AssignedUsersGridViewModel();

            cachedViewModel.OfferId = offerId;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                var linkedTable = context.OffersApplicationUsers.Where(x => x.OfferId == offerId);

                var query = linkedTable.Select(x => x.ApplicationUser).Select(x => new AssignedUserViewModel
                {
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    NumberOfBooking = linkedTable.FirstOrDefault(y=>y.ApplicationUserId == x.Id).BookingCount,
                    Status = linkedTable.FirstOrDefault(y => y.ApplicationUserId == x.Id).Status,
                    OfferId = offerId,
                });

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
                ViewBag.ItemNumber = query.Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower())
                                           || s.LastName.ToLower().Contains(searchString.ToLower())
                                           || s.UserName.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder == "IdDesc" ? "Id" : "IdDesc";
                ViewBag.FirstNameSortParm = sortOrder == "firstName" ? "firstName_desc" : "firstName";
                ViewBag.UserNameSortParm = sortOrder == "userName" ? "userNameDesc" : "userName";
                ViewBag.LastNameSortParm = sortOrder == "lastName" ? "lastNameDesc" : "lastName";
                ViewBag.EmailSortParm = sortOrder == "email" ? "emailDesc" : "email";
                ViewBag.BookingNumber = sortOrder == "bookingNumber" ? "bookingNumberDesc" : "bookingNumber";
                switch (sortOrder)
                {
                    case "userName":
                        query = query.OrderBy(s => s.UserName);
                        break;
                    case "userNameDesc":
                        query = query.OrderByDescending(s => s.UserName);
                        break;
                    case "firstName":
                        query = query.OrderBy(s => s.FirstName);
                        break;
                    case "firstName_desc":
                        query = query.OrderByDescending(s => s.FirstName);
                        break;
                    case "lastName":
                        query = query.OrderBy(s => s.LastName);
                        break;
                    case "lastNameDesc":
                        query = query.OrderByDescending(s => s.LastName);
                        break;
                    case "email":
                        query = query.OrderBy(s => s.Email);
                        break;
                    case "emailDesc":
                        query = query.OrderByDescending(s => s.Email);
                        break;
                    case "bookingNumber":
                        query = query.OrderBy(s => s.NumberOfBooking);
                        break;
                    case "bookingNumberDesc":
                        query = query.OrderByDescending(s => s.NumberOfBooking);
                        break;
                    default:
                        query = query.OrderByDescending(s => s.UserName);
                        break;
                }

                #endregion Sort Search

                int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
                int pageNumber = (page ?? 1);
                ViewBag.PageSize = pageSize;
                var tempList = query.ToList();
                cachedViewModel.AssignedUserList = tempList.ToPagedList(pageNumber, pageSize);
                ViewBag.ItemNumber = cachedViewModel.AssignedUserList.Count();
            }

            return PartialView("~/Views/Offers/AssignedUsers.cshtml", cachedViewModel);
        }

        [HttpPost]
        public ActionResult BookTrip(int bookingCount, int offerId)
        {
            if (!ModelState.IsValid)
            {

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
                        offerApplicationUser.Status = "Niezaakceptowany";
                    }
                    //new
                    else
                    {
                        var offerApplicationUser = new OffersApplicationUsers
                        {
                            ApplicationUser = currentUser,
                            Offers = currentOffert,
                            BookingCount = bookingCount,
                            Status = "Niezaakceptowany",
                        };
                        context.OffersApplicationUsers.Add(offerApplicationUser);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
            return RedirectToAction("GetOffersDetail", "Offers" ,new { id = offerId });
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
            var model = offerServices.GetOfferDetail(id,User.Identity.Name);
            string userName = User.Identity.Name;
            model.IsUser = userServices.IsUser(userName);
            model.IsTravelAgency= userServices.IsTravelAgency(userName);
            model.RandomOffers = offerServices.GetRandomOffers(id);
            if (model.IsTravelAgency)
            {
                model.IsOwner = userServices.IsOwner(id, User.Identity.Name);
            }
            model.IsFinished = DateTime.Now > model.StartDate;
            return View("~/Views/Offers/OfferDetailInfo.cshtml", model);
        }

        [HttpPost]
        public ActionResult DisableOffer(int offerId)
        {
            bool isOwner = userServices.IsOwner(offerId, User.Identity.Name);
            if (isOwner)
            {
                var result = offerServices.DisableOffer(offerId);
            }
            return RedirectToAction("GetYourOffers");
        }
        [HttpPost]
        public ActionResult EnableOffer(int offerId)
        {
            bool isOwner = userServices.IsOwner(offerId, User.Identity.Name);
            if (isOwner)
            {
                var result = offerServices.EnableOffer(offerId);
            }
            return RedirectToAction("GetYourOffers");
        }
        [HttpPost]
        public ActionResult RemoveOffer(int offerId)
        {
            bool isOwner = userServices.IsOwner(offerId, User.Identity.Name);
            if (isOwner)
            {
                var result = offerServices.RemoveOffer(offerId);
            }
            return RedirectToAction("GetYourOffers");
        }
        [HttpPost]
        public ActionResult DuplicateOffer(int offerId)
        {
            bool isOwner = userServices.IsOwner(offerId, User.Identity.Name);
            if (isOwner)
            {
                var result = offerServices.DuplicateOffer(offerId);
            }
            return RedirectToAction("GetYourOffers");
        }

        [HttpPost]
        public ActionResult ApproveBooking(string userName , int offerId)
        {
            var result = offerServices.ApproveBooking(userName, offerId);

            return RedirectToAction("GetOffersDetail", new { id = offerId });
        }
        [HttpPost]
        public ActionResult RejectBooking(string userName, int offerId)
        {
            var result = offerServices.RejectBooking(userName,offerId);

            return RedirectToAction("GetOffersDetail", new { id = offerId });
        }

        [HttpPost]
        public ActionResult EditOffer(int offerId)
        {
            bool isOwner = userServices.IsOwner(offerId, User.Identity.Name);
            OfferDetailViewModel model = new OfferDetailViewModel();
            model = offerServices.GetOfferDetail(offerId,User.Identity.Name);
            return View("~/Views/Offers/EditOffer.cshtml",model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditOfferSubmit(OfferDetailViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool success = offerServices.EditOffer(model, User.Identity.Name);
                return RedirectToAction("GetYourOffers");
            }
            return View("~/Views/Offers/EditOffer.cshtml", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateComments(CreateCommentToOfferViewModel model ,int offerId)
        {

            if (ModelState.IsValid)
            {
                offerServices.CreateComment(model, offerId, User.Identity.Name);
            }
            return RedirectToAction("GetOffersDetail",new { id = offerId });
        }

    }
}