using JourneyPortal.Services;
using JourneyPortal.ViewModels.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourneyPortal.Controllers
{
    public class HomeController : Controller
    {
        OfferServices offerServices;
        public HomeController()
        {
            offerServices = new OfferServices();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchOffert(DateTime? startDate , DateTime? endDate , int? minPrice , int? maxPrice , string country , int? bookingNumber , string activeOffert)
        {
            var model = offerServices.SearchOffers(startDate, endDate, minPrice, maxPrice, country, bookingNumber, activeOffert);
            return View("~/Views/Home/SearchResult.cshtml",model);
        }
    }
}