﻿using JourneyPortal.Models;
using JourneyPortal.Services;
using JourneyPortal.ViewModels.Trip;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    [Authorize]
    public class TripController : Controller
    {
        public TripService tripService { get; set; }
        public UserServices userServices { get; set; }

        public TripController()
        {
            tripService = new TripService();
            userServices = new UserServices();
        }

        public ActionResult Index()
        {
            return View("~/Views/Trip/Index.cshtml");
        }

        public ActionResult ManageAtractions(int? page)
        {
            var model = new ManageAtractionsViewModel();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            model.atractionsList = tripService.PrepareAtractionList(User.Identity.Name).ToPagedList(pageNumber, pageSize);
            return View("~/Views/Trip/ManageAtractions.cshtml",model);
        }

        [HttpGet]
        public ActionResult CreateNewAtraction()
        {
            var cachedViewModel = new CreateNewAtractionViewModel();
            return View("~/Views/Trip/CreateNewAtraction.cshtml", cachedViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewAtraction(CreateNewAtractionViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                bool success = tripService.CreateNewAtraction(model, User.Identity.Name, file, this);
                return RedirectToAction("ManageAtractions");
            }
            return View("~/Views/Trip/CreateNewAtraction.cshtml", model);
        }


        [HttpPost]
        public ActionResult EditAtraction(int atractionId)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            AtractionDetailViewModel model = new AtractionDetailViewModel();
            model = tripService.GetAtractionDetail(atractionId, User.Identity.Name);
            return View("~/Views/Trip/EditAtraction.cshtml", model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditAtractionSubmit(AtractionDetailViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool success = tripService.EditAtraction(model, User.Identity.Name);
                return RedirectToAction("ManageAtractions");
            }
            return View("~/Views/Trip/EditAtraction.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetAtractionDetail(int id)
        {
            var userName = User.Identity.Name;
            var model = tripService.GetAtractionDetail(id, userName);
            model.IsOwner = userServices.IsAtractionOwner(id, User.Identity.Name);
            return View("~/Views/Trip/AtractionDetail.cshtml", model);
        }

        [HttpPost]
        public ActionResult DisableAtractions(int atractionId)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.DisableAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions");
        }
        [HttpPost]
        public ActionResult EnableAtractions(int atractionId)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.EnableAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions");
        }
        [HttpPost]
        public ActionResult RemoveAtractions(int atractionId)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.RemoveAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions");
        }

        public ActionResult ManageHotels(int? page)
        {
            var model = new ManageHotelsViewModel();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            model.hotelList = tripService.PrepareHotelList(User.Identity.Name).ToPagedList(pageNumber,pageSize);
            return View("~/Views/Trip/ManageHotels.cshtml",model);
        }

        [HttpGet]
        public ActionResult CreateNewHotel()
        {
            var cachedViewModel = new CreateNewHotelViewModel();
            return View("~/Views/Trip/CreateNewHotel.cshtml", cachedViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewHotel(CreateNewHotelViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                bool success = tripService.CreateNewHotel(model, User.Identity.Name, file, this);
                return RedirectToAction("ManageHotels");
            }
            return View("~/Views/Trip/CreateNewHotel.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditAtractionImage(HttpPostedFileBase file, int atractionId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var currentAtraction = context.Atractions.FirstOrDefault(x => x.Id == atractionId);

                Guid id = Guid.NewGuid();
                if (file != null)
                {
                    Image image = new Image();
                    var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                    image.ImageUrl = file.ToString();
                    image.Name = currentAtraction.Name + id + "-image";
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + image.Name + ext;
                        var path = Path.Combine("~/Content/AtractionsImages", myfile);
                        image.ImageUrl = path;
                        context.Images.Add(image);
                        file.SaveAs(Server.MapPath(path));
                    }
                    currentAtraction.Image = image.ImageUrl;
                }
                else
                {
                    var image = context.Images.FirstOrDefault(x => x.ImageUrl == currentAtraction.Image);
                    context.Images.Remove(image);
                    currentAtraction.Image = null;
                }
                context.SaveChanges();
            }
            return RedirectToAction("GetAtractionDetail", "Trip", new { id = atractionId });
        }

        [HttpPost]
        public ActionResult EditHotelImage(HttpPostedFileBase file, int hotelId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var currentHotel = context.Hotels.FirstOrDefault(x => x.Id == hotelId);

                Guid id = Guid.NewGuid();
                if (file != null)
                {
                    Image image = new Image();
                    var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                    image.ImageUrl = file.ToString();
                    image.Name = currentHotel.Name + id + "-image";
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + image.Name + ext;
                        var path = Path.Combine("~/Content/HotelsImages", myfile);
                        image.ImageUrl = path;
                        context.Images.Add(image);
                        file.SaveAs(Server.MapPath(path));
                    }
                    currentHotel.Image = image.ImageUrl;
                }
                else
                {
                    var image = context.Images.FirstOrDefault(x => x.ImageUrl == currentHotel.Image);
                    context.Images.Remove(image);
                    currentHotel.Image = null;
                }
                context.SaveChanges();
            }
            return RedirectToAction("GetHotelDetail", "Trip", new { id = hotelId });
        }

        [HttpPost]
        public ActionResult EditHotel(int hotelId)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            HotelDetailViewModel model = new HotelDetailViewModel();
            model = tripService.GetHotelDetail(hotelId, User.Identity.Name);
            return View("~/Views/Trip/EditHotel.cshtml", model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditHotelSubmit(HotelDetailViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool success = tripService.EditHotel(model, User.Identity.Name);
                return RedirectToAction("ManageHotels");
            }
            return View("~/Views/Trip/EditHotel.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetHotelDetail(int id)
        {
            var userName = User.Identity.Name;
            var model = tripService.GetHotelDetail(id, userName);
            model.IsOwner = userServices.IsHotelOwner(id, User.Identity.Name);
            return View("~/Views/Trip/HotelDetail.cshtml", model);
        }

        [HttpPost]
        public ActionResult DisableHotel(int hotelId)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.DisableHotel(hotelId);
            }
            return RedirectToAction("ManageHotels");
        }
        [HttpPost]
        public ActionResult EnableHotel(int hotelId)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.EnableHotel(hotelId);
            }
            return RedirectToAction("ManageHotels");
        }
        [HttpPost]
        public ActionResult RemoveHotel(int hotelId)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.RemoveHotel(hotelId);
            }
            return RedirectToAction("ManageHotels");
        }

        [HttpGet]
        public ActionResult GetAllAtractions()
        {
            var atractionList = tripService.GetAllAtractions();
            return new JsonResult
            {
                Data = new
                {
                    atraction = atractionList,
                    success = true,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult GetAllHotels()
        {
            var hotelList = tripService.GetAllHotels();
            return new JsonResult
            {
                Data = new
                {
                    hotels = hotelList,
                    success = true,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult GetSampleRoute()
        {
            var route = tripService.GetSampleRoute();
            return new JsonResult
            {
                Data = new
                {
                    route = route,
                    success = true,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}