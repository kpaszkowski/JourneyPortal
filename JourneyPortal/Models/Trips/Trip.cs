﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models.Trips
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TouristFacility> TouristFacilities { get; set; }

        public int BaseHotelId { get; set; }
        public TouristFacility BaseHotel { get; set; }

        public string CreatedById{ get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public double TotalDistance { get; set; }
        public int NumberOfDays{ get; set; }
    }
}