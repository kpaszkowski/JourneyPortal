using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastActivity { get; set; }
        public IPagedList<TopicsGridViewModel> TopicsList { get; set; }
        public bool isAdmin { get; set; }
    }
}