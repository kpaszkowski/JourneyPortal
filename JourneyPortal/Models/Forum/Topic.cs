using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Forum
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDisabled { get; set; }
        public int Views { get; set; }
        public ApplicationUser Author { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}