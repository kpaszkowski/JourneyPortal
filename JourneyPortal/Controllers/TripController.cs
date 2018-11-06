using JourneyPortal.Models;
using JourneyPortal.Services;
using JourneyPortal.ViewModels.Shared;
using JourneyPortal.ViewModels.Trip;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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

        public ActionResult Index(int? tripId)
        {
            var model = new MapViewModel();
            model.IsProprietor = userServices.IsProprietor(User.Identity.Name);
            model.IsUser = userServices.IsUser(User.Identity.Name);
            if (tripId != null)
            {
                model.DrawMap = true;
                tripService.PrepareSavedTrip(model, tripId);
            }
            return View("~/Views/Trip/Index.cshtml",model);
        }

        public ActionResult ManageAtractions(int? page)
        {
            var model = new ManageAtractionsViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            model.Page = pageNumber;
            model.atractionsList = tripService.PrepareAtractionList(User.Identity.Name).ToPagedList(pageNumber, pageSize);
            return View("~/Views/Trip/ManageAtractions.cshtml",model);
        }

        [HttpPost]
        public ActionResult AddObjectFromMap(string radio ,string xCoords, string yCoords,string name)
        {
            bool success;
            string text = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                name = "Nowy";
            }
            if (radio == "hotel")
            {
                var model = new CreateNewHotelViewModel
                {
                    Name = name,
                    CostPerNight = 0,
                    Description = "Nowy",
                    X = double.Parse(xCoords, CultureInfo.InvariantCulture),
                    Y = double.Parse(yCoords, CultureInfo.InvariantCulture)
                };
                success = tripService.CreateNewHotel(model, User.Identity.Name, null, this);
                text = "Pomyślnie dodano hotel.Teraz może go aktywować w sekcji Zarządzaj hotelami";
            }
            else
            {
                var model = new CreateNewAtractionViewModel()
                {
                    Name = name,
                    Description = "Nowa",
                    Cost = 0,
                    TimeOfSightseeing = 0,
                    Type = "Nowa",
                    X = double.Parse(xCoords, CultureInfo.InvariantCulture),
                    Y = double.Parse(yCoords, CultureInfo.InvariantCulture)
                };
                success = tripService.CreateNewAtraction(model, User.Identity.Name,null, this);
                text = "Pomyślnie dodano atrakcję.Teraz może ją aktywować w sekcji Zarządzaj atrakcjami";
            }
            return Json
            (
                new
                {
                    x = xCoords,
                    y = yCoords,
                    text = text,
                    isHotel = radio == "hotel" ? true:false,
                    name = name,
                    success = success,
                }
            );
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
        public ActionResult DisableAtractions(int atractionId,int page)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.DisableAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions",new { page =  page});
        }
        [HttpPost]
        public ActionResult EnableAtractions(int atractionId, int page)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.EnableAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions", new { page = page });
        }
        [HttpPost]
        public ActionResult RemoveAtractions(int atractionId,int page)
        {
            bool isOwner = userServices.IsAtractionOwner(atractionId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.RemoveAtractions(atractionId);
            }
            return RedirectToAction("ManageAtractions", new { page = page });
        }

        public ActionResult ManageHotels(int? page)
        {
            var model = new ManageHotelsViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            model.Page = pageNumber;
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
            model = tripService.GetHotelDetail(hotelId);
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
            var model = tripService.GetHotelDetail(id);
            model.IsOwner = userServices.IsHotelOwner(id, User.Identity.Name);
            return View("~/Views/Trip/HotelDetail.cshtml", model);
        }

        [HttpPost]
        public ActionResult DisableHotel(int hotelId, int page)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.DisableHotel(hotelId);
            }
            return RedirectToAction("ManageHotels", new { page = page });
        }
        [HttpPost]
        public ActionResult EnableHotel(int hotelId, int page)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.EnableHotel(hotelId);
            }
            return RedirectToAction("ManageHotels", new { page = page });
        }
        [HttpPost]
        public ActionResult RemoveHotel(int hotelId, int page)
        {
            bool isOwner = userServices.IsHotelOwner(hotelId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.RemoveHotel(hotelId);
            }
            return RedirectToAction("ManageHotels",new { page = page });
        }

        [HttpPost]
        public ActionResult RemoveTrip(int tripId,int page)
        {
            bool isOwner = userServices.IsTripOwner(tripId, User.Identity.Name);
            if (isOwner)
            {
                var result = tripService.RemoveTrip(tripId);
            }
            return RedirectToAction("GetYourTrip",new { page = page });
        }

        [HttpGet]
        public ActionResult GetNearestAtractions(double x, double y)
        {
            Point point = new Point(x, y);
            var atractionList = tripService.GetNearestAtractions(point);

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
        public ActionResult GetYourTouristFacilities()
        {
            var hotelList = tripService.GetYourHotels(User.Identity.Name);
            var atractionList = tripService.GetYourAtractions(User.Identity.Name);
            return new JsonResult
            {
                Data = new
                {
                    hotels = hotelList,
                    atractions = atractionList,
                    success = true,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult SaveTrip(string route ,string travelDistance,string travelDuration,string travelDurationTraffic, string name,string hotel,string atractions)
        {
            var selectedHotel = JsonConvert.DeserializeObject<SelectedTouristFacilitiesViewModel>(hotel);
            var selectedAtractions = JsonConvert.DeserializeObject<List<SelectedTouristFacilitiesViewModel>>(atractions);
            var routes = JsonConvert.DeserializeObject<List<RouteViewModel>>(route);
            tripService.CreateNewTrip(selectedHotel, selectedAtractions,routes, name, User.Identity.Name, travelDistance,travelDuration,travelDurationTraffic);
            return new JsonResult
            {
                Data = new
                {
                    success = true,
                },
            };
        }

        [HttpGet]
        public ActionResult GetYourTrip(int? page)
        {
            var model = new ManageTripViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            model.Page = pageNumber;
            model.tripList = tripService.PrepareTripList(User.Identity.Name).ToPagedList(pageNumber, pageSize);
            return View("~/Views/Trip/ManageTrips.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTripDetail(int tripId,int? page)
        {
            var model = new TripDetailViewModel();
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);
            int pageNumber = (page ?? 1);
            model = tripService.PrepareTripDetail(tripId);
            model.AtractionsList = tripService.PrepareAtractionListInTrip(tripId).ToPagedList(pageNumber, pageSize);
            return View("~/Views/Trip/TripDetail.cshtml", model);
        }

    }
}