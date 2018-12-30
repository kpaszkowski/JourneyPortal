using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using JourneyPortal.Controllers;
using JourneyPortal.ViewModels.Chat;
using JourneyPortal.ViewModels.Forum;
using JourneyPortal.ViewModels.Offers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestJourneyPortal
{
    [TestClass]
    public class UnitTest1
    {
        public OffersController offerController { get; set; }
        public ChatController chatController { get; set; }
        public ForumController forumController { get; set; }
        public TripController tripController { get; set; }

        public UnitTest1()
        {
            offerController = new OffersController();
            chatController = new ChatController();
            forumController = new ForumController();
            tripController = new TripController();

            var fakeContext = CreateFakeContext("User").Object;

            offerController.ControllerContext = fakeContext;
            chatController.ControllerContext = fakeContext;
            forumController.ControllerContext = fakeContext;
            tripController.ControllerContext = fakeContext;
        }

        private Mock<ControllerContext> CreateFakeContext(string userName)
        {
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity(userName);
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            return controllerContext;
        }

        [TestMethod]
        public void OffersView()
        {
            var result = offerController.CreateNewOffert() as ViewResult;
            Assert.AreEqual("~/Views/Offers/CreateNewOffert.cshtml", result.ViewName);
            Assert.IsTrue(result.Model is CreateOfferDetailViewModel);

            var result2 = offerController.GetComments(1,1) as PartialViewResult;
            Assert.AreEqual("~/Views/Offers/CommentsGrid.cshtml", result2.ViewName);
            Assert.IsTrue(result2.Model is ComentsGridViewModel);

            result = offerController.Index() as ViewResult;
            Assert.AreEqual("~/Views/Offers/Index.cshtml", result.ViewName);
            Assert.IsTrue(result.Model is OffersViewModel);

            result = offerController.EditOffer(1) as ViewResult;
            Assert.AreEqual("~/Views/Offers/EditOffer.cshtml", result.ViewName);
            Assert.IsTrue(result.Model is OffersViewModel);
        }
        [TestMethod]
        public void OffersAction()
        {
            var result = (RedirectToRouteResult)offerController.DisableOffer(1);
            Assert.AreEqual("GetYourOffers", result.RouteValues["action"]);

            result = (RedirectToRouteResult)offerController.DisableOffer(1);
            Assert.AreEqual("GetYourOffers", result.RouteValues["action"]);

            result = (RedirectToRouteResult)offerController.EnableOffer(1);
            Assert.AreEqual("GetYourOffers", result.RouteValues["action"]);

            result = (RedirectToRouteResult)offerController.RemoveOffer(1);
            Assert.AreEqual("GetYourOffers", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ChatView()
        {
            var result = chatController.Index() as ViewResult;
            Assert.AreEqual("~/Views/Chat/Index.cshtml", result.ViewName);
            Assert.IsTrue(result.Model is ChatViewModel);

            
        }
        [TestMethod]
        public void ChatAction()
        {
            var result = (RedirectToRouteResult)chatController.CreateNewMessage(new GlobalMessageViewModel { Text = "test" });
            Assert.AreEqual("Index", result.RouteValues["action"]);

            var jsonResult = chatController.GetMessages() as JsonResult;
            Assert.IsTrue(jsonResult.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

        }

        [TestMethod]
        public void ForumView()
        {
            var result = forumController.Index(1) as ViewResult;
            Assert.AreEqual("~/Views/Forum/Index.cshtml", result.ViewName);
            Assert.IsTrue(result.Model is ForumViewModel);

            result = forumController.CreateNewCategory() as ViewResult;
            Assert.AreEqual("~/Views/Forum/CreateNewCategory.cshtml", result.ViewName);

        }

        [TestMethod]
        public void ForumAction()
        {
            var result = (RedirectToRouteResult)forumController.CreatePost(new CreatePostViewModel { Text = "test" },1,1);
            Assert.AreEqual("GetPosts", result.RouteValues["action"]);

        }

    }
}
