using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Shared
{
    public class Point
    {
        public Point()
        {

        }
        public Point(double x, double y)
        {
            this.latitude = x;
            this.longitude = y;
        }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public void Copy(Point point)
        {
            this.latitude = point.latitude;
            this.longitude = point.longitude;
        }
    }
}