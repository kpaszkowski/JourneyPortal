using JourneyPortal.Controllers;
using JourneyPortal.Models;
using JourneyPortal.Models.Offer;
using JourneyPortal.ViewModels.Offers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JourneyPortal.Services
{
    public class OfferServices
    {
        internal bool SignOff(int offerId, int? numberOfBooking , string userName)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var currentUser = userManager.FindByName(userName);

                    var currentOffer = context.Offers.FirstOrDefault(x => x.Id == offerId);

                    var connectingTable = context.OffersApplicationUsers.FirstOrDefault(x => x.ApplicationUserId == currentUser.Id && x.OfferId == offerId);

                    if (connectingTable.BookingCount == numberOfBooking)
                    {
                        context.OffersApplicationUsers.Remove(connectingTable);
                    }
                    else
                    {
                        connectingTable.BookingCount -= numberOfBooking ?? 0;
                    }
                    currentOffer.NuberOfBooking += numberOfBooking ?? 0;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        internal List<OfferDetailViewModel> GetAllOffers()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Offers.Select(x => new OfferDetailViewModel
                    {
                        Id = x.Id,
                        Cost = x.Cost,
                        CreationDate = x.CreationDate,
                        Description = x.Description,
                        EndDate = x.EndDate,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        Country = x.Country,
                        TravelAgencyOwnerName = x.TravelAgencyOwner.UserName,
                        Rate = x.Rate,
                        IsActive = x.IsActive,
                        Image = x.Image,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool CreateNewOffert(CreateOfferDetailViewModel model, string userName, HttpPostedFileBase file,OffersController offersController)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    Offers newOffert = new Offers()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        NuberOfBooking = model.NuberOfBooking,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Cost = model.Cost,
                        CreationDate = DateTime.Now,
                        Country = model.Country,
                        TravelAgencyOwner = userManager.FindByName(userName),
                    };

                    Image image = new Image();
                    var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg",".ico"
                    };
                    Guid id = Guid.NewGuid();
                    image.ImageUrl = file.ToString();
                    image.Name = newOffert.Name + id + "-image";
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + image.Name + ext;
                        var path = Path.Combine(offersController.Server.MapPath("~/Content/OffersImages"), myfile);
                        image.ImageUrl = path;
                        context.Images.Add(image);
                        file.SaveAs(path);
                    }
                    newOffert.Image = image.ImageUrl;
                    context.Offers.Add(newOffert);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal OfferDetailViewModel GetOfferDetail(int id,string userName)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                int n = 0;
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(userName);
                if (currentUser != null)
                {
                    n = context.OffersApplicationUsers.Where(y => y.ApplicationUserId == currentUser.Id && y.OfferId == id).Select(x=>x.BookingCount).FirstOrDefault();
                }
                return context.Offers.Where(x => x.Id == id).Select(x => new OfferDetailViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreationDate = x.CreationDate,
                    NuberOfBooking = x.NuberOfBooking,
                    TravelAgencyOwnerName = x.TravelAgencyOwner.UserName,
                    Cost = x.Cost,
                    Country = x.Country,
                    Rate = x.Rate,
                    Image = x.Image,
                    NumberOfUserCurrentBooking = n,
                }).FirstOrDefault();
            }
        }

        internal bool DisableOffer(int offerId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Offers.FirstOrDefault(x => x.Id == offerId).IsActive = false;
                context.SaveChanges();
            }
            return true;
        }

        internal bool EnableOffer(int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Offers.FirstOrDefault(x => x.Id == offerId).IsActive = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal bool RemoveOffer(int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var offerToRemove = context.Offers.FirstOrDefault(x => x.Id == offerId);
                    context.Offers.Remove(offerToRemove);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal object DuplicateOffer(int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var offerToDuplicate = context.Offers.FirstOrDefault(x => x.Id == offerId);
                    Offers offer = new Offers();
                    offer.Update(offerToDuplicate);
                    context.Offers.Add(offer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal bool EditOffer(OfferDetailViewModel model, string name)
        {

            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentOffer = context.Offers.FirstOrDefault(x => x.Id == model.Id);
                    currentOffer.Name = model.Name;
                    currentOffer.Description = model.Description;
                    currentOffer.NuberOfBooking = model.NuberOfBooking;
                    currentOffer.StartDate = model.StartDate;
                    currentOffer.EndDate = model.EndDate;
                    currentOffer.Cost = model.Cost;
                    currentOffer.Country = model.Country;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void CreateComment(CreateCommentToOfferViewModel model, int offerId, string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var comments = new OffersComment
                    {
                        Text = model.Text,
                        Like = 0,
                        Author = context.Users.FirstOrDefault(x => x.UserName == name),
                        CreationDate = DateTime.Now,
                        Rate = model.Rate,
                        Offers = context.Offers.FirstOrDefault(x=>x.Id == offerId),
                        
                    };
                    var currentOffer = context.Offers.FirstOrDefault(x => x.Id == offerId);

                    var currentOfferComments = context.OffersComments.Where(x => x.Offers.Id == currentOffer.Id).ToList();
                    int noc = currentOfferComments.Count;
                    int roc = currentOfferComments.Sum(x => x.Rate);


                    currentOffer.Rate = (double)(roc + comments.Rate) / (double)(noc + 1);
                    context.OffersComments.Add(comments);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}