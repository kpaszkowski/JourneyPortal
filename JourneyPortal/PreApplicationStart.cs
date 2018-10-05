using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal
{
    public class PreApplicationStart
    {
        public static void Start()
        {
            System.Web.Security.Roles.Enabled = true;
        }
    }
}