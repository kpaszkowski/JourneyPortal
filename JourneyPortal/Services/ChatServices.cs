using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using JourneyPortal.ViewModels.Chat;

namespace JourneyPortal.Services
{
    public class ChatServices
    {
        public UserServices userService { get; set; }

        public ChatServices()
        {
            userService = new UserServices();
        }

        internal List<UserChatViewModel> GetUsers(string name)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    var users = context.Users.Where(x=>x.UserName != name).Select(x => new UserChatViewModel
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                    }).ToList();

                    foreach (var item in users)
                    {
                        item.UserRole = userService.GetUserRole(item.UserName);
                    }

                    return users;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal MessageViewModel GetMessages(string userId, string name)
        {
            //var user2Id = userService.GetUserId(name);
            //var IdList = new List<string>();
            //IdList.Add(userId);
            //IdList.Add(user2Id);
            //try
            //{
            //    using (ApplicationDbContext context = new ApplicationDbContext())
            //    {
            //        var user1Messages 

            //        var users = context.ChatMessages.Where(x => id).Select(x => new
            //        {
            //            Id = x.Id,
            //            AuthorName = x.a
            //        }).ToList();

            //        return users;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return null;
        }
    }
}