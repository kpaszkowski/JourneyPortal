﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Forum
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int Like { get; set; }
        public ApplicationUser Author { get; set; }
        public Topic Topic { get; set; }

        public virtual ICollection<PostsUsers> PostsUsers { get; set; }
    }
}