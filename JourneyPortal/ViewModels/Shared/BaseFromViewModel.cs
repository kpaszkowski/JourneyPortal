using JourneyPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Shared
{
    public class BaseFormViewModel : ISessionCacheObject
    {
        public long ID { get; set; }

        public bool IsEdit { get; set; }

        #region ControllerNames

        public virtual string ControllerName { get; set; }

        public virtual string AreaName { get; set; }

        #endregion

        #region SessiontCache

        public string SessionCacheKey { get; set; }


        public bool IgnoreSessionTimeout
        {
            get { return false; }
        }

        #endregion SessiontCache
    }
}