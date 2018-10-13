using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Forum
{
    public class PostsUsers
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Post Post { get; set; }
    }
}