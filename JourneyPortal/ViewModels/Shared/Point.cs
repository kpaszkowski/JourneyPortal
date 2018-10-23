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
            this.X = x;
            this.Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public void Copy(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }
    }
}