using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Offers
{
    public class ComentsGridViewModel
    {
        public int OfferId { get; set; }
        public bool IsAdmin { get; set; }
        public IPagedList<CommentsViewModel> CommentsList { get; set; }
    }
}