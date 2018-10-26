using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using JourneyPortal.Models.Chat;
using JourneyPortal.ViewModels.Chat;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

        internal ConversationViewModel StartConversation(string userId1, string userId2)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    ApplicationUser user1 = userManager.FindById(userId1);
                    ApplicationUser user2 = userManager.FindById(userId2);

                    Conversation conversation = new Conversation
                    {
                        FirstUser = user1,
                        SecondUser = user2,
                    };
                    context.Conversations.Add(conversation);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return null;
        }

        internal ConversationViewModel GetConversation(string userId, string name)
        {
            ConversationViewModel conversationViewModel;
            try
            {
                var user2Id = userService.GetUserId(name);
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    conversationViewModel = context.Conversations.Where(x => (x.FirstUserId == userId && x.SecondUserId == user2Id) ||
                    (x.SecondUserId == userId && x.FirstUserId == user2Id)).Select(x => new ConversationViewModel
                    {
                        Id = x.Id,
                        FirstUserId = x.FirstUserId,
                        FistUserName = x.FirstUser.UserName,
                        SecondUserId = x.SecondUserId,
                        SecondUserName = x.SecondUser.UserName,
                        IsActive = true,
                       
                    }).FirstOrDefault();
                    if (conversationViewModel == null)
                    {
                        conversationViewModel = new ConversationViewModel
                        {
                            FirstUserId = userId,
                            SecondUserId = user2Id,
                            FistUserName = userService.GetUserName(userId),
                            SecondUserName = name,
                            Messages = new List<MessageViewModel>(),
                            IsActive = false
                        };
                        
                        
                    }
                    else
                    {
                        conversationViewModel.Messages = GetMessages(conversationViewModel.Id);
                    }
                    return conversationViewModel;
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<MessageViewModel> GetMessages(int id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Messages.Where(x => x.Conversation.Id == id).Select(x => new MessageViewModel
                    {
                        Id = x.Id,
                        AuthorName = x.Author.UserName,
                        CreationDate = x.CreationDate,
                        Text = x.Text,
                        AuthorImage = x.Author.Avatar
                        
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}