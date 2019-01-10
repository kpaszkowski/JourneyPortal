using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public byte[] Binary { get; set; }
    }
}