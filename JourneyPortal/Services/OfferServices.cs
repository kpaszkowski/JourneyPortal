using JourneyPortal.Models;
using JourneyPortal.Models.Offer;
using JourneyPortal.ViewModels.Offers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
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

        internal List<OffersGridViewModel> GetAllOffers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Offers.Select(x => new OffersGridViewModel
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
                }).ToList();
            }
        }

        internal bool CreateNewOffert(CreateOfferDetailViewModel model, string userName)
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

        internal List<OffersGridViewModel> GetOffersForTravelAgency(string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindByName(name);

                return context.Offers.Where(x=>x.TravelAgencyOwnerId == user.Id).Select(x => new OffersGridViewModel
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
                    IsActive = x.IsActive,
                    Rate = x.Rate,
                }).ToList();
            }
        }

        internal OfferDetailViewModel GetOfferDetail(int id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
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
    }
}