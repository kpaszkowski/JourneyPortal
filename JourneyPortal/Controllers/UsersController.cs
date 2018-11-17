using JourneyPortal.Models;
using JourneyPortal.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using JourneyPortal.ViewModels.Users;
using JourneyPortal.Services;
using System.IO;
using PagedList;
using System.Configuration;

namespace JourneyPortal.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> userManager;
        UserServices userServices;

        public UsersController()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userServices = new UserServices();
        }
        // GET: Users
        public ActionResult Index()
        {
            var cachedViewModel = new UserProfileInfo();
            if (User.Identity.IsAuthenticated)
            {
                SessionCache.Set(cachedViewModel);
                var u = userManager.FindByName(User.Identity.Name.ToString());
                cachedViewModel.UpdateFromModel(u);

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return PartialView("~/Views/Users/Index.cshtml",cachedViewModel);
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return PartialView("~/Views/Users/Index.cshtml",cachedViewModel);
        }

        public ActionResult ManageUsers(int? page)
        {
            var model = new ManageUsersViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            model.usersList = userServices.GetAllUsers().ToPagedList(pageNumber, pageSize);
            model.RolesList = userServices.GetAllRoles();
            return View("~/Views/Users/UserList.cshtml", model);
        }

        public ActionResult ChangeUserRole(string userId,string newRole)
        {
            userServices.ChangeUserRole(userId, newRole);
            return RedirectToAction("ManageUsers", "Users");
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Any())
                {
                    if (s[0].ToString() == "Admin")
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetTravelAgencyProfile(string userName)
        {
            var cachedViewModel = new TravelAgencyInfoViewModel();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                cachedViewModel = context.Users.Where(x => x.UserName == userName).Select(x => new TravelAgencyInfoViewModel
                {
                    Email = x.Email,
                    Name = x.UserName,
                    OffersList = x.OwnerOffers.Select(y => new ViewModels.Offers.OfferDetailViewModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Description = y.Description,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        CreationDate = y.CreationDate,
                        Cost = y.Cost,
                        NuberOfBooking = y.NuberOfBooking,
                        IsActive = y.IsActive,
                        Image = y.Image,
                        Rate = y.Rate
                    }).ToList()
                }).FirstOrDefault();

            }

            return View("~/Views/Users/GetTravelAgencyProfile.cshtml", cachedViewModel);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsTravelAgency()
        {
            bool isTravelAgency;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                isTravelAgency = userManager.IsInRole(User.Identity.GetUserId(),"TravelAgency");
            }

            return Json(new { isTravelAgency = isTravelAgency },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditProfileData()
        {
            var model = new EditUserProfileViewModel();
            model = userServices.PrepareEditUserProfile(User.Identity.Name);
            return View("~/Views/Users/EditProfileData.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditProfileData(EditUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = userServices.EditUserProfile(model);
                return RedirectToAction("Index", "Manage");
            }
            return View("~/Views/Users/EditProfileData.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditAvatar(HttpPostedFileBase file)
        {
            if (file != null)
            {
                Image image = new Image();
                var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                };
                var userManagers = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = userManagers.FindByName(User.Identity.Name);
                image.ImageUrl = file.ToString();
                image.Name = currentUser.UserName + "-avatar";
                var fileName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(file.FileName);
                if (allowedExtensions.Contains(ext))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string myfile = name + "_" + image.Name + ext;
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), myfile);
                    image.ImageUrl = path;
                    context.Images.Add(image);
                    currentUser.Avatar = image.ImageUrl;
                    context.SaveChanges();
                    file.SaveAs(path);
                }
            }
            
            return RedirectToAction("Index", "Manage");
        }

    }
}