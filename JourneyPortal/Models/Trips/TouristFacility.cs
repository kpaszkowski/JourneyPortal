using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class TouristFacility
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string OwnerId { get; set; }
        public string Image { get; set; }
        public ApplicationUser Owner { get; set; }

    }
}