using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using JourneyPortal.Controllers;
using JourneyPortal.Models;
using JourneyPortal.Models.Trips;
using JourneyPortal.ViewModels.Shared;
using JourneyPortal.ViewModels.Trip;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace JourneyPortal.Services
{
    public class TripService
    {
        internal List<HotelGridViewModel> PrepareHotelList(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(name);
                    return context.Hotels.Where(x => x.OwnerId == currentUser.Id).Select(x => new HotelGridViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CostPerNight = x.CostPerNight,
                        IsActive = x.IsActive,
                        X = x.X,
                        Y = x.Y,
                        Rate = x.Rate,
                        OwnerEmail = currentUser.Email
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void PrepareSavedTrip(MapViewModel model, int? tripId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    model.Hotel = context.Trips.Where(x => x.Id == tripId).Select(x => new HotelGridViewModel
                    {
                        Id = x.BaseHotel.Id,
                        Name = x.BaseHotel.Name,
                        CostPerNight = x.BaseHotel.CostPerNight,
                        OwnerEmail = x.BaseHotel.Owner.Email,
                        Rate = x.BaseHotel.Rate,
                        X = x.BaseHotel.X,
                        Y = x.BaseHotel.Y,
                        IsActive = x.BaseHotel.IsActive,
                    }).FirstOrDefault();
                    model.Atractions = PrepareAtractionListInTrip(tripId ?? default(int));
                    model.Routes = context.Routes.Where(x => x.TripId == tripId).Select(x => new RouteViewModel
                    {
                        Start = new Point
                        {
                            latitude = x.StartX,
                            longitude = x.StartY,
                        },
                        End = new Point
                        {
                            latitude = x.EndX,
                            longitude = x.EndY,
                        }

                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal List<AtractionsGridViewModel> PrepareAtractionList(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(name);
                    return context.Atractions.Where(x => x.OwnerId == currentUser.Id).Select(x => new AtractionsGridViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Cost = x.Cost,
                        IsActive = x.IsActive,
                        X = x.X,
                        Y = x.Y,
                        Rate = x.Rate,
                        OwnerEmail = currentUser.Email,
                        Type = x.Type,
                        TimeOfSightseeing = x.TimeOfSightseeing,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool EditAtraction(AtractionDetailViewModel model, string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var currentAtracttion = context.Atractions.FirstOrDefault(x => x.Id == model.Id);
                    currentAtracttion.Name = model.Name;
                    currentAtracttion.Description = model.Description;
                    currentAtracttion.Cost = model.Cost;
                    currentAtracttion.TimeOfSightseeing = model.TimeOfSightseeing;
                    currentAtracttion.Type = model.Type;
                    currentAtracttion.X = model.X;
                    currentAtracttion.Y = model.Y;
                    context.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal AtractionDetailViewModel GetAtractionDetail(int id, string userName)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Atractions.Where(x => x.Id == id).Select(x => new AtractionDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        Cost = x.Cost,
                        OwnerEmail = x.Owner.Email,
                        OwnerName = x.Owner.UserName,
                        Rate = x.Rate,
                        TimeOfSightseeing = x.TimeOfSightseeing,
                        Type = x.Type,
                        X = x .X,
                        Y = x.Y,
                        Image = x.Image
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal object RemoveAtractions(int atractionId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var atractionToRemove = context.Atractions.FirstOrDefault(x => x.Id == atractionId);
                    context.Atractions.Remove(atractionToRemove);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal bool CreateNewHotel(CreateNewHotelViewModel model, string userName, HttpPostedFileBase file, TripController tripController)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var currentUser = userManager.FindByName(userName);

                    Hotel newHotel = new Hotel()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CostPerNight = model.CostPerNight,
                        Owner = currentUser,
                        X = model.X,
                        Y = model.Y
                    };
                    if (file != null)
                    {
                        Image image = new Image();
                        var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                        Guid id = Guid.NewGuid();
                        image.ImageUrl = file.ToString();
                        image.Name = newHotel.Name + id + "-image";
                        var fileName = Path.GetFileName(file.FileName);
                        var ext = Path.GetExtension(file.FileName);
                        if (allowedExtensions.Contains(ext))
                        {
                            string name = Path.GetFileNameWithoutExtension(fileName);
                            string myfile = name + "_" + image.Name + ext;
                            var path = Path.Combine(tripController.Server.MapPath("~/Content/HotelsImages"), myfile);
                            image.ImageUrl = path;
                            context.Images.Add(image);
                            file.SaveAs(path);
                        }
                        newHotel.Image = image.ImageUrl;
                    }
                    context.Hotels.Add(newHotel);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal object RemoveHotel(int hotelId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var hotel = context.Hotels.FirstOrDefault(x => x.Id == hotelId);
                    context.Hotels.Remove(hotel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal object EnableHotel(int hotelId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var hotel = context.Hotels.FirstOrDefault(x => x.Id == hotelId);
                    hotel.IsActive = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal object DisableHotel(int hotelId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var hotel = context.Hotels.FirstOrDefault(x => x.Id == hotelId);
                    hotel.IsActive = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal bool EditHotel(HotelDetailViewModel model, string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var currentHotel = context.Hotels.FirstOrDefault(x => x.Id == model.Id);
                    currentHotel.Name = model.Name;
                    currentHotel.Description = model.Description;
                    currentHotel.CostPerNight = model.CostPerNight;
                    currentHotel.X = model.X;
                    currentHotel.Y = model.Y;
                    context.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal HotelDetailViewModel GetHotelDetail(int hotelId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Hotels.Where(x => x.Id == hotelId).Select(x => new HotelDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        CostPerNight = x.CostPerNight,
                        OwnerEmail = x.Owner.Email,
                        OwnerName = x.Owner.UserName,
                        Rate = x.Rate,
                        X = x.X,
                        Y = x.Y,
                        Image = x.Image,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool EnableAtractions(int atractionId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var atraction = context.Atractions.FirstOrDefault(x => x.Id == atractionId);
                    atraction.IsActive = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal dynamic GetNearestAtractions(Point point)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    double kmLimit = double.Parse(ConfigurationManager.AppSettings["KmLimit"]);
                    Func<Point, Point ,bool> isInLimit = (point1 ,point2) =>
                    {
                        var result = Math.Sqrt(Math.Pow(point2.latitude - point1.latitude, 2) + Math.Pow(Math.Cos((point1.latitude * Math.PI) / 180) * (point2.longitude - point1.longitude), 2)) * (40075.704 / 360);
                        return result <= kmLimit;
                    };

                    var allAtractions = context.Atractions.ToList();
                    return allAtractions.Where(x => isInLimit(new Point { latitude = x.X, longitude = x.Y }, point)).Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                        Type = x.Type,
                        Rate = x.Rate,
                    }).ToList();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal dynamic GetAllAtractions()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Atractions.Where(x=>x.IsActive).Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                        Type = x.Type,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal dynamic GetSampleRoute()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Atractions.Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal object RemoveTrip(int tripId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var query = context.Trips.Where(x => x.Id == tripId);
                    var tripToRemove = query.FirstOrDefault();
                    foreach (var item in query.SelectMany(x=>x.Atractions))
                    {
                        item.Rate--;
                    }
                    context.Hotels.FirstOrDefault(x => x.Id == tripToRemove.BaseHotelId).Rate--;
                    context.Trips.Remove(tripToRemove);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal dynamic GetAllHotels()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Hotels.Where(x => x.IsActive).Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool DisableAtractions(int atractionId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var atraction = context.Atractions.FirstOrDefault(x => x.Id == atractionId);
                    atraction.IsActive = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal dynamic GetYourHotels(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Hotels.Where(x => x.Owner.UserName == name).Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        Rate = x.Rate,
                        Cost = x.CostPerNight,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void CreateNewTrip(SelectedTouristFacilitiesViewModel selectedHotel, List<SelectedTouristFacilitiesViewModel> selectedAtractions, List<RouteViewModel> routes, string name, string userName, string travelDistance, string travelDuration, string travelDurationTraffic)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    ApplicationUser currentUser = userManager.FindByName(userName);                    
                    Trip trip = new Trip();
                    List<Atraction> atractions = new List<Atraction>();
                    foreach (var item in selectedAtractions)
                    {
                        atractions.Add(context.Atractions.FirstOrDefault(x => x.Id == item.IdDb));
                        context.Atractions.Where(x => x.Id == item.IdDb).FirstOrDefault().Rate++;
                    }
                    foreach (var item in routes)
                    {
                        Route route = new Route
                        {
                            Trip = trip,
                            StartX = item.Start.latitude,
                            StartY = item.Start.longitude,
                            EndX = item.End.latitude,
                            EndY = item.End.longitude,
                        };
                        context.Routes.Add(route);
                    }
                    context.Hotels.FirstOrDefault(x => x.Id == selectedHotel.IdDb).Rate++;
                    trip.Atractions = atractions;
                    trip.BaseHotel = context.Hotels.FirstOrDefault(x => x.Id == selectedHotel.IdDb);
                    trip.Name = name;
                    trip.CreatedBy = currentUser;
                    trip.TotalDistance = double.Parse(travelDistance, CultureInfo.InvariantCulture);
                    trip.Duration = Int32.Parse(travelDuration);
                    trip.DurationTraffic = Int32.Parse(travelDurationTraffic, CultureInfo.InvariantCulture);

                    context.Trips.Add(trip);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal List<AtractionsGridViewModel> PrepareAtractionListInTrip(int tripId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var atraction = context.Trips.Where(x => x.Id == tripId).SelectMany(x=>x.Atractions).ToList();
                    return atraction.Select(x => new AtractionsGridViewModel
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Cost = x.Cost,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        Rate = x.Rate,
                        TimeOfSightseeing = x.TimeOfSightseeing,
                        Type = x.Type,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal List<AtractionDetailViewModel> PrepareAtractionListInTripInfo(int tripId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var atraction = context.Trips.Where(x => x.Id == tripId).SelectMany(x => x.Atractions).ToList();
                    return atraction.Select(x => new AtractionDetailViewModel
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Cost = x.Cost,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        Rate = x.Rate,
                        TimeOfSightseeing = x.TimeOfSightseeing,
                        Type = x.Type,
                        Image = x.Image,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal TripDetailViewModel PrepareTripDetail(int tripId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Trips.Where(x => x.Id == tripId).Select(x => new TripDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        AtractionNumber = x.Atractions.Count(),
                        BaseHotel = new HotelDetailViewModel {
                            Id = x.BaseHotel.Id,
                            Name = x.BaseHotel.Name,
                            CostPerNight = x.BaseHotel.CostPerNight,
                            OwnerEmail = x.BaseHotel.Owner.Email,
                            Rate = x.BaseHotel.Rate,
                            X = x.BaseHotel.X,
                            Y = x.BaseHotel.Y,
                            IsActive = x.BaseHotel.IsActive,
                            Image = x.BaseHotel.Image
                        },
                        Duration = x.Duration,
                        DurationTrafiic = x.DurationTraffic,
                        TotalDistance = x.TotalDistance,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<TripGridViewModel> PrepareTripList(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(name);
                    return context.Trips.Where(x => x.CreatedById == currentUser.Id).Select(x => new TripGridViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Duration = x.Duration,
                        DurationTrafiic = x.DurationTraffic,
                        TotalDistance = x.TotalDistance,
                        AtractionNumber = x.Atractions.Count(),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal dynamic GetYourAtractions(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    return context.Atractions.Where(x => x.Owner.UserName == name).Select(x => new
                    {
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                        Name = x.Name,
                        Type = x.Type,
                        IsActive = x.IsActive,
                        Rate = x.Rate,
                        Cost = x.Cost,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool CreateNewAtraction(CreateNewAtractionViewModel model, string userName, HttpPostedFileBase file, TripController tripController)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var currentUser = userManager.FindByName(userName); 

                    Atraction newAtraction = new Atraction()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Cost = model.Cost,
                        TimeOfSightseeing = model.TimeOfSightseeing,
                        Owner = currentUser,
                        Type = model.Type,
                        X = model.X,
                        Y = model.Y
                    };
                    if (file !=null)
                    {
                        Image image = new Image();
                        var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                        Guid id = Guid.NewGuid();
                        image.ImageUrl = file.ToString();
                        image.Name = newAtraction.Name + id + "-image";
                        var fileName = Path.GetFileName(file.FileName);
                        var ext = Path.GetExtension(file.FileName);
                        if (allowedExtensions.Contains(ext))
                        {
                            string name = Path.GetFileNameWithoutExtension(fileName);
                            string myfile = name + "_" + image.Name + ext;
                            var path = Path.Combine(tripController.Server.MapPath("~/Content/AtractionsImages"), myfile);
                            image.ImageUrl = path;
                            context.Images.Add(image);
                            file.SaveAs(path);
                        }
                        newAtraction.Image = image.ImageUrl;
                    }
                    context.Atractions.Add(newAtraction);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}