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
        ForumService forumService;

        public ForumController()
        {
            userServices = new UserServices();
            forumService = new ForumService();
            
        }

        public ActionResult Index()
        {
            var model = new ForumViewModel();
            if (userServices.GetUserRole(User.Identity.Name) == "Admin")
            {
                model.IsAdmin = true;
            }
            model.Categories = forumService.GetAllCategories();

            return View("~/Views/Forum/Index.cshtml",model);
        }

        [Authorize]
        public ActionResult CreateNewCategory()
        {
            return View("~/Views/Forum/CreateNewCategory.cshtml");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateNewCategory(CreateCategoryViewModel model)
        {
            forumService.CreateNewCategory(model);
            return RedirectToAction("Index", "Forum");
        }

        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreatePost(CreatePostViewModel model , int topicId , int categoryId)
        {
            forumService.CreatePost(model,topicId,categoryId,User.Identity.Name);
            return RedirectToAction("Index", "Forum");
        }

        [HttpGet]
        public ActionResult GetTopics(int categoryId)
        {
            var model = new CategoryDetailsViewModel();
            forumService.PrepareCategoriesDetailsViewModel(model,categoryId);
            model.TopicsList = forumService.GetTopicsFor(categoryId);
            return View("~/Views/Forum/CategoryDetails.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLike(int postId)
        {
            var model = new PostViewModel();
            forumService.AddLike(postId);
            return RedirectToAction("Index", "Forum");
        }

        [HttpGet]
        public ActionResult GetPosts(int topicId , int categoryId)
        {
            var model = new TopicDetailsViewModel();
            forumService.PrepareTopicsDetailsViewModel(model, topicId ,categoryId);
            model.PostsList = forumService.GetPostsFor(topicId);
            return View("~/Views/Forum/TopicDetails.cshtml", model);
        }

        [Authorize]
        public ActionResult CreateNewTopic(int categoryId)
        {
            var model = new CreateTopicViewModel();
            model.Id = categoryId;
            return View("~/Views/Forum/CreateNewTopic.cshtml",model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateNewTopic(CreateTopicViewModel model , int categoryId)
        {
            forumService.CreateNewTopic(model, categoryId);
            return RedirectToAction("Index", "Forum");
        }
    }
}