using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Forum
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
        public DateTime LastActivity { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }

    }
}