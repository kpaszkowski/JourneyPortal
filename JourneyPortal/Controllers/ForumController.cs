using JourneyPortal.Models;
using JourneyPortal.Services;
using JourneyPortal.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        UserServices userServices;

        public ForumController()
        {
            userServices = new UserServices();
            
        }
        public ActionResult Index()
        {
            var model = new ForumViewModel();
            if (userServices.GetUserRole(User.Identity.Name) == "Admin")
            {
                model.IsAdmin = true;
            }
            model.Categories = new List<CategoryGridViewModel>();

            return View("~/Views/Forum/Index.cshtml",model);
        }
        public ActionResult CreateNewCategory()
        {
            return View("~/Views/Forum/CreateNewCategory.cshtml");
        }
        [HttpPost]
        public ActionResult CreateNewCategory(CreateCategoryViewModel model)
        {
            return RedirectToAction("Index", "Forum");
        }
    }
}