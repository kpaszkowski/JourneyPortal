using System;
using System.Collections.Generic;
using System.Configuration;
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

        internal HotelDetailViewModel GetHotelDetail(int hotelId, string name)
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
                        var result = Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(Math.Cos((point1.X * Math.PI) / 180) * (point2.Y - point1.Y), 2)) * (40075.704 / 360);
                        return result <= kmLimit;
                    };

                    var allAtractions = context.Atractions.ToList();
                    return allAtractions.Where(x => isInLimit(new Point { X = x.X, Y = x.Y }, point)).Select(x => new
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