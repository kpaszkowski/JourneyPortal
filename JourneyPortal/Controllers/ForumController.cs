using JourneyPortal.Models;
using JourneyPortal.Services;
using JourneyPortal.ViewModels.Forum;
using PagedList;
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

        public ActionResult Index(int? page)
        {
            var model = new ForumViewModel();
            if (userServices.IsAdmin(User.Identity.Name))
            {
                model.IsAdmin = true;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            model.Categories = forumService.GetAllCategories().ToPagedList(pageNumber,pageSize);

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
            if (ModelState.IsValid)
            {
                forumService.CreateNewCategory(model);
                return RedirectToAction("Index", "Forum");
            }
            return View(model);
        }

        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreatePost(CreatePostViewModel model , int topicId , int categoryId)
        {
            if (ModelState.IsValid)
            {
                forumService.CreatePost(model,topicId,categoryId,User.Identity.Name);
            }
            return RedirectToAction("GetPosts","Forum",new { topicId = topicId , categoryId = categoryId});
        }

        [HttpGet]
        public ActionResult GetTopics(int categoryId , int? page)
        {
            var model = new CategoryDetailsViewModel();
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            forumService.PrepareCategoriesDetailsViewModel(model,categoryId);
            model.TopicsList = forumService.GetTopicsFor(categoryId).ToPagedList(pageNumber,pageSize);
            return View("~/Views/Forum/CategoryDetails.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLike(int postId, int topicId, int categoryId)
        {
            var model = new PostViewModel();
            var result = forumService.AddLike(postId,User.Identity.Name);
            return RedirectToAction("GetPosts", "Forum", new { topicId = topicId, categoryId = categoryId });
        }

        [HttpGet]
        public ActionResult GetPosts(int topicId , int categoryId,int? page)
        {
            var model = new TopicDetailsViewModel();
            forumService.PrepareTopicsDetailsViewModel(model, topicId ,categoryId);
            model.isAdmin = userServices.IsAdmin(User.Identity.Name);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            model.PostsList = forumService.GetPostsFor(topicId).ToPagedList(pageNumber, pageSize);
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
            if (ModelState.IsValid)
            {
                forumService.CreateNewTopic(model, categoryId);
                return RedirectToAction("GetTopics", "Forum", new { categoryId = categoryId });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RemovePost(int postId, int topicId, int categoryId)
        {
            bool isAdmin = userServices.IsAdmin(User.Identity.Name);
            if (isAdmin)
            {
                var result = forumService.RemovePost(postId);
            }
            return RedirectToAction("GetPosts", "Forum", new { topicId = topicId, categoryId = categoryId });
        }
    }
}