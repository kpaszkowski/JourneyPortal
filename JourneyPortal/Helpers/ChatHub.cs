using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;

namespace JourneyPortal.Helpers
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    ApplicationUser currentUser = userManager.FindByName(name);
                    GlobalMessage globalMessage = new GlobalMessage
                    {
                        Text = message,
                        CreationDate = DateTime.Now,
                        Author = currentUser
                    };
                    context.GlobalMessages.Add(globalMessage);
                    context.SaveChanges();
                    string avatar = string.Empty;
                    if (currentUser.Avatar != null)
                    {
                        avatar ="/Content/Images/" + Path.GetFileName(currentUser.Avatar);
                    }
                    Clients.All.addNewMessageToPage(name, message,globalMessage.CreationDate.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, avatar);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}