using System;
using System.Collections.Specialized;

namespace JourneyPortal.Helpers
{

    public interface ISessionCacheObject
    {
        string SessionCacheKey { get; set; }
        bool IgnoreSessionTimeout { get; }
    }

    public static class SessionCache
    {

        private const string sessionKey = "SessionCache";

        public static int SessionTimeout = 20;

        public static T Get<T>(string key)
            where T : class, ISessionCacheObject
        {
            string cache_key;
            T result;
            if (string.IsNullOrWhiteSpace(key))
            {
                result = null;
                throw new SessionCacheException("Brak klucza do cache'u sesji.");
            }
            else
            {
                cache_key = typeof(T).FullName;
                SessionCacheContainer cache = System.Web.HttpContext.Current.Session[sessionKey] as SessionCacheContainer ?? new SessionCacheContainer();
                System.Web.HttpContext.Current.Session[sessionKey] = cache;

                SessionCacheDictionary dict = cache.Cache[cache_key] as SessionCacheDictionary ?? new SessionCacheDictionary();
                cache.Cache[cache_key] = dict;
                dict.LastAccessed = DateTime.Now;
                ClearOldCache();

                result = dict.Dictionary[key] as T;
            }

            if (result == null)
            {
                throw new SessionCacheException("Brak obiektu w cache'u sesji. Typ obiektu: " + cache_key);
            }

            return result;
        }

        public static string Set<T>(T obj, string key = null)
            where T : class, ISessionCacheObject
        {
            string cache_key = typeof(T).FullName;
            SessionCacheContainer cache = System.Web.HttpContext.Current.Session[sessionKey] as SessionCacheContainer ?? new SessionCacheContainer();
            System.Web.HttpContext.Current.Session[sessionKey] = cache;

            SessionCacheDictionary dict = cache.Cache[cache_key] as SessionCacheDictionary ?? new SessionCacheDictionary();
            dict.LastAccessed = DateTime.Now;
            ClearOldCache();
            cache.Cache[cache_key] = dict;

            if (key == null)
            {
                key = obj.SessionCacheKey;

                if (key == null)
                {
                    key = Guid.NewGuid().ToString();
                }
            }

            dict.Dictionary[key] = obj;

            if (obj != null)
            {
                obj.SessionCacheKey = key;
            }

            if (dict.Dictionary.Count > 10)
            {
                dict.Dictionary.RemoveAt(0);
            }

            return key;
        }

        /// <summary>
        /// Clears cache older than 20 minutes.
        /// </summary>
        public static void ClearOldCache()
        {
            SessionCacheContainer cache = System.Web.HttpContext.Current.Session[sessionKey] as SessionCacheContainer;

            if (cache == null)
            {
                return;
            }

            DateTime now = DateTime.Now;

            for (int i = cache.Cache.Count - 1; i >= 0; i--)
            {
                ISessionCacheDictionary dict = (ISessionCacheDictionary)cache.Cache[i];

                if ((now - dict.LastAccessed).Minutes > SessionTimeout)
                {
                    cache.Cache.RemoveAt(i);
                }
            }
        }

        #region Internal classes
        /// <summary>
        /// Session cache dictionary.
        /// </summary>
        interface ISessionCacheDictionary
        {
            /// <summary>
            /// Last access date.
            /// </summary>
            DateTime LastAccessed { get; set; }

            /// <summary>
            /// Dictionary with elemenets.
            /// </summary>
            OrderedDictionary Dictionary { get; set; }
        }

        /// <summary>
        /// Session cache dictionary.
        /// </summary>
        class SessionCacheDictionary : ISessionCacheDictionary
        {
            /// <summary>
            /// Last access date.
            /// </summary>
            public DateTime LastAccessed { get; set; }

            /// <summary>
            /// Dictionary with elemenets.
            /// </summary>
            public OrderedDictionary Dictionary { get; set; }

            /// <summary>
            /// Inicjalizuje nową instancję klasy <see cref="SessionCacheDictionary"/>. 
            /// Creates new instance of SessionCacheDictionary class.
            /// </summary>
            public SessionCacheDictionary()
            {
                LastAccessed = DateTime.Now;
                Dictionary = new OrderedDictionary(10);
            }
        }

        /// <summary>
        /// Session cache container.
        /// </summary>
        class SessionCacheContainer
        {
            /// <summary>
            /// Dictionary with caches dictionaries.
            /// </summary>
            public OrderedDictionary Cache { get; set; }

            /// <summary>
            /// Inicjalizuje nową instancję klasy <see cref="SessionCacheContainer"/>. 
            /// Creates new instance of SessionCacheContainer class.
            /// </summary>
            public SessionCacheContainer()
            {
                this.Cache = new OrderedDictionary();
            }
        }
        #endregion Internal classes
    }

    public class SessionCacheException : Exception
    {
        public SessionCacheException(string message) : base(message)
        {
        }
    }
}