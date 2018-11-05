using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using JourneyPortal.ViewModels.Chat;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JourneyPortal.Services
{
    public class ChatService
    {
        internal List<GlobalMessageViewModel> GetAllMessages()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.GlobalMessages.Select(x => new GlobalMessageViewModel
                    {
                        Id = x.Id,
                        Text = x.Text,
                        CreationDate = x.CreationDate,
                        AuthorName = x.Author.UserName,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void CreateNewMessage(string text, string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var currentUser = userManager.FindByName(name);
                    GlobalMessage message = new GlobalMessage
                    {
                        Text = text,
                        CreationDate = DateTime.Now,
                        Author = currentUser,
                    };
                    context.GlobalMessages.Add(message);
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