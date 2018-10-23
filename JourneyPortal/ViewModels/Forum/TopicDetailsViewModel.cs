using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class TopicDetailsViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastActivity { get; set; }
        public IPagedList<PostViewModel> PostsList { get; set; }
        public bool isAdmin { get; set; }
    }
}