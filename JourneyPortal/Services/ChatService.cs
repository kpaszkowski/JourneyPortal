using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using JourneyPortal.ViewModels.Chat;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace JourneyPortal.Services
{
    public class ChatService
    {
        internal dynamic GetAllMessages()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var globalMessagesList = context.GlobalMessages.Select(x => new GlobalMessageViewModel
                    {
                        Id = x.Id,
                        Text = x.Text,
                        AuthorName = x.Author.UserName,
                        AuthorAvatar = x.Author.Image != null ? x.Author.Image.Binary : null,
                        CreationDate = x.CreationDate
                    }).ToList();

                    foreach (var item in globalMessagesList)
                    {
                        if (item.AuthorAvatar != null)
                        {
                            item.AuthorAvatarJson = JsonConvert.SerializeObject(Convert.ToBase64String(item.AuthorAvatar));
                            item.AuthorAvatar = null;
                        }
                        item.DateTimeSeconds = item.CreationDate.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                    }
                    return globalMessagesList;
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