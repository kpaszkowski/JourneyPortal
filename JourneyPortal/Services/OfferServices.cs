using JourneyPortal.Controllers;
using JourneyPortal.Helpers;
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
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        internal List<OfferDetailViewModel> SearchOffers(DateTime? startDate, DateTime? endDate, int? minPrice, int? maxPrice, string country, int? bookingNumber, string activeOffert)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    bool active = activeOffert == null ? false:true;
                    if (startDate == null)
                    {
                        startDate = DateTime.MinValue;
                    }
                    if (endDate == null)
                    {
                        endDate = DateTime.MaxValue;
                    } 
                    if (minPrice == null)
                    {
                        minPrice = Int32.MinValue;
                    }
                    if (maxPrice == null)
                    {
                        maxPrice = Int32.MaxValue;
                    }
                    if (bookingNumber == null)
                    {
                        bookingNumber = 0;
                    }
                    if (country == null)
                    {
                        country = string.Empty;
                    }
                    return context.Offers.Where(x => x.Country.Contains(country) && (x.Cost >= minPrice && x.Cost <= maxPrice)
                            && (x.EndDate <= endDate && x.StartDate >= startDate )&& x.NuberOfBooking >= bookingNumber && (active == true ? DateTime.Now <= x.StartDate: true)).Select(x => new OfferDetailViewModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                Cost = x.Cost,
                                Rate = x.Rate,
                                Country = x.Country,
                                NuberOfBooking = x.NuberOfBooking,
                                IsActive = x.IsActive,
                                Image = x.Image != null ? x.Image.Binary : null,
                                CreationDate = x.CreationDate,
                                IsFinished = (DateTime.Now > x.StartDate),
                            }).ToList();
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
                        Image = x.Image != null ? x.Image.Binary : null,
                        IsFinished = (DateTime.Now > x.StartDate)
                    }).OrderBy(x => x.IsFinished).ToList();
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

                    if (file != null)
                    {
                        var image = ImageHelper.PrepareImage(file);
                        context.Images.Add(image);
                        newOffert.Image = image;
                    }
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
                bool can = false;
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = userManager.FindByName(userName);
                var currentLinkTable = context.OffersApplicationUsers.Where(y => y.ApplicationUserId == currentUser.Id && y.OfferId == id);
                if (currentUser != null)
                {
                    n = currentLinkTable.Select(x=>x.BookingCount).FirstOrDefault();
                    if (currentLinkTable.Any())
                    {
                        if (currentLinkTable.FirstOrDefault().Status=="Zaakceptowany" && context.Offers.FirstOrDefault(x=>x.Id == id).EndDate <= DateTime.Now)
                        {
                            can = true;
                        }
                    }
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
                    Image = x.Image != null ? x.Image.Binary : null,
                    NumberOfUserCurrentBooking = n,
                    CanAddComment = can,
                    IsActive = x.IsActive
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

        internal bool ApproveBooking(string userName, int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(userName);
                    var currentLinkTable = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id && y.OfferId == offerId);
                    currentLinkTable.Status = "Zaakceptowany";
                    context.Offers.FirstOrDefault(x=>x.Id == offerId).NuberOfBooking -= currentLinkTable.BookingCount;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool RejectBooking(string userName, int offerId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(userName);
                    var currentLinkTable = context.OffersApplicationUsers.FirstOrDefault(y => y.ApplicationUserId == currentUser.Id && y.OfferId == offerId);
                    if (currentLinkTable.Status == "Zaakceptowany")
                    {
                        context.Offers.FirstOrDefault(x => x.Id == offerId).NuberOfBooking += currentLinkTable.BookingCount;
                    }
                    currentLinkTable.Status = "Odrzucony";
                    context.OffersApplicationUsers.Remove(currentLinkTable);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal List<OfferDetailViewModel> GetRandomOffers(int id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    Random rand = new Random();
                    var ownerName = context.Offers.Where(x => x.Id == id).Select(x => x.TravelAgencyOwner.UserName).FirstOrDefault();
                    var randomOffers = context.Offers.Where(x => x.IsActive && x.Id != id && x.TravelAgencyOwner.UserName == ownerName).Select(x=> new OfferDetailViewModel
                    {
                        Id = x.Id,
                        Cost = x.Cost,
                        Country = x.Country,
                        CreationDate = x.CreationDate,
                        EndDate =x.EndDate,
                        StartDate = x.StartDate,
                        Rate = x.Rate,
                        Image = x.Image != null ? x.Image.Binary : null,
                        Name = x.Name,
                        TravelAgencyOwnerName = x.TravelAgencyOwner.UserName,
                        IsFinished = (DateTime.Now > x.StartDate)
                    }).ToList();
                    List<OfferDetailViewModel> newList = new List<OfferDetailViewModel>();
                    int loopConstraint = randomOffers.Count > 3 ? 3 : randomOffers.Count;
                    for (int i = 0; i < loopConstraint; i++)
                    {
                        int index = rand.Next(randomOffers.Count);
                        newList.Add(randomOffers[index]);
                        randomOffers.RemoveAt(index);
                    }
                    return newList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}