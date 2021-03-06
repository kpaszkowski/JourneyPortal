﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JourneyPortal.Models;
using JourneyPortal.Models.Forum;
using JourneyPortal.ViewModels.Forum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JourneyPortal.Services
{
    public class ForumService
    {
        internal List<CategoryGridViewModel> GetAllCategories()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Categories.Select(x => new CategoryGridViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    LastActivity = x.LastActivity,
                }).ToList();

            }
        }

        internal void CreateNewCategory(CreateCategoryViewModel model)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var category = new Category
                    {
                        Name = model.Name,
                        Description = model.Description,
                        LastActivity = DateTime.Now,
                    };
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void CreatePost(CreatePostViewModel model, int topicId, int categoryId, string userName)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var currentTopic = context.Topics.FirstOrDefault(x => x.Id == topicId);
                    var currentCategory = context.Categories.FirstOrDefault(x => x.Id == categoryId);
                    currentTopic.LastActivity = DateTime.Now;
                    currentCategory.LastActivity = DateTime.Now;
                    var post = new Post
                    {
                        Text = model.Text,
                        Like = 0,
                        Author = context.Users.FirstOrDefault(x => x.UserName == userName),
                        CreationDate = DateTime.Now,
                        Topic = currentTopic,
                        };
                    context.Posts.Add(post);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool AddLike(int postId,string userName)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var userId = userManager.FindByName(userName).Id;
                    if (!context.PostsUsers.Where(x=>x.PostId==postId && x.ApplicationUserId == userId).Any())
                    {
                        if (context.Posts.Where(x => x.Id == postId).Select(x => x.Author.Id).FirstOrDefault() != userId)
                        {
                            context.Posts.FirstOrDefault(x => x.Id == postId).Like++;
                            PostsUsers postsUsers = new PostsUsers
                            {
                                ApplicationUserId = userId,
                                PostId = postId,
                            };
                            context.PostsUsers.Add(postsUsers);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
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

        internal object RemovePost(int postId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var postToRemove = context.Posts.FirstOrDefault(x => x.Id == postId);
                    context.Posts.Remove(postToRemove);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal List<TopicsGridViewModel> GetTopicsFor(int categoryId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Topics.Where(x=>x.Category.Id == categoryId).Select(x => new TopicsGridViewModel
                {
                    Id = x.Id,
                    CategoryId = categoryId,
                    Name = x.Name,
                    Description = x.Description,
                    LastActivity = x.LastActivity,
                    Views = x.Views,
                }).ToList();

            }
        }

        internal List<PostViewModel> GetPostsFor(int topicId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Topics.FirstOrDefault(x => x.Id == topicId).Views++;
                var posts = context.Posts.Where(x => x.Topic.Id == topicId).Select(x => new PostViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    Likes = x.Like,
                    CreationDate = x.CreationDate
                }).ToList();

                context.SaveChanges();
                return posts;
            }
        }

        internal void PrepareTopicsDetailsViewModel(TopicDetailsViewModel model, int topicId, int categoryId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var topic = context.Topics.FirstOrDefault(x => x.Id == topicId);
                model.Id = topic.Id;
                model.Name = topic.Name;
                model.Description = topic.Description;
                model.LastActivity = topic.LastActivity;
                model.CategoryId = categoryId;
            }
        }

        internal void PrepareCategoriesDetailsViewModel(CategoryDetailsViewModel model, int categoryId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == categoryId);
                model.Id = category.Id;
                model.Name = category.Name;
                model.Description = category.Description;
                model.LastActivity = category.LastActivity;
            }
        }

        internal void CreateNewTopic(CreateTopicViewModel model, int categoryId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var currentCategory = context.Categories.FirstOrDefault(x => x.Id == categoryId);
                    currentCategory.LastActivity = DateTime.Now;
                    var topic = new Topic
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CreatedDate = DateTime.Now,
                        LastActivity = DateTime.Now,
                        Category = currentCategory,
                    };
                    context.Topics.Add(topic);
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