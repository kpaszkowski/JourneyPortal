using JourneyPortal.Models;
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
    }
}